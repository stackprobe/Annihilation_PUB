using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HLTStudio.Commons;
using HLTStudio.Tools;

namespace HLTStudio
{
	public class LaunchSequence
	{
		public void Run()
		{
			GetRawKey(); // 鍵ファイルの存在チェック

			string revisionCode = DownloadRevisionCode();

			if (
				!IsGameInstalled() ||
				IsGameUpdated(revisionCode)
				)
				InstallGame(revisionCode);

			BootGame();
		}

		private string DownloadRevisionCode()
		{
			using (WorkingDir wd = new WorkingDir())
			{
				string revisionCodeFile = wd.MakePath();

				HTTPClient hc = new HTTPClient(Consts.URL_REVISION_CODE)
				{
					ConnectTimeoutMillis = 20000,
					TimeoutMillis = 25000,
					IdleTimeoutMillis = 15000,
					ResBodySizeMax = 300,
					ResFile = revisionCodeFile,
				};

				hc.Get();

				string revisionCode = SCommon.ToJString(File.ReadAllBytes(revisionCodeFile), false, false, false, false);
				return revisionCode;
			}
		}

		private bool IsGameInstalled()
		{
			return File.Exists(Consts.GAME_PROGRAM);
		}

		private bool IsGameUpdated(string revisonCode)
		{
			if (!File.Exists(Consts.LAST_REVISION_CODE_FILE))
				return true;

			string lastRevisionCode = File.ReadAllText(Consts.LAST_REVISION_CODE_FILE, Encoding.ASCII).Trim();
			return lastRevisionCode != revisonCode;
		}

		private void InstallGame(string revisionCode)
		{
			try
			{
				SCommon.DeletePath(Consts.LAST_REVISION_CODE_FILE);
			}
			catch (Exception ex)
			{
				throw new Exception($"{Path.GetFileName(Consts.LAST_REVISION_CODE_FILE)} ファイルは使用中です。", ex);
			}

			if (Directory.Exists(Consts.GAME_DIR))
			{
				string escapeDir = SCommon.ToCreatablePath(Consts.GAME_DIR);

				try
				{
					Directory.Move(Consts.GAME_DIR, escapeDir);
				}
				catch (Exception ex)
				{
					throw new Exception($"{Path.GetFileName(Consts.GAME_DIR)} フォルダは使用中です。", ex);
				}

				SCommon.DeletePath(escapeDir);
			}

			using (WorkingDir wd = new WorkingDir())
			{
				string gameClusterFile = wd.MakePath();

				HTTPClient hc = new HTTPClient(Consts.URL_GAME)
				{
					ConnectTimeoutMillis = 20000,
					TimeoutMillis = 900_000,
					IdleTimeoutMillis = 30000,
					ResBodySizeMax = 8_000_000_000,
					ResFile = gameClusterFile,
				};

				hc.Get();

				new RingCipherFile(GetRawKey()).Decrypt(gameClusterFile);

				//SCommon.DeleteAndCreateDir(Consts.GAME_DIR);
				DirToClusterFileTools.ClusterFileToDir(gameClusterFile, Consts.GAME_DIR);
			}

			File.WriteAllText(Consts.LAST_REVISION_CODE_FILE, revisionCode);
		}

		private byte[] GetRawKey()
		{
			string[] files = Directory.GetFiles(Consts.KEYS_DIR, "*.xml");

			if (files.Length == 0)
				throw new Exception("鍵ファイルがありません！\r\n[[ Readme.txt ]] に従って鍵ファイルを配置してください。");

			if (files.Length != 1)
				throw new Exception("鍵ファイルを特定できませんでした。");

			string file = files[0];

			XMLTools.Node root = XMLTools.LoadFromFile(file);

			byte[] key = SCommon.Hex.I.GetBytes(new string(root["key"].Value
				.Where(chr => ' ' < chr)
				.ToArray()));

			byte[] hash = SCommon.Hex.I.GetBytes(new string(root["hash"].Value
				.Where(chr => ' ' < chr)
				.ToArray()));

			byte[] calcedHash = SCommon.GetSHA256(key);

			if (SCommon.Comp(hash, calcedHash, SCommon.Comp) != 0)
				throw new Exception("Bad hash");

			return key;
		}

		private void BootGame()
		{
			SCommon.Batch(new string[]
			{
				$"START \"\" \"{Path.GetFileName(Consts.GAME_PROGRAM)}\"",
			}
			, Consts.GAME_DIR
			, SCommon.StartProcessWindowStyle_e.MINIMIZED
			);
		}
	}
}
