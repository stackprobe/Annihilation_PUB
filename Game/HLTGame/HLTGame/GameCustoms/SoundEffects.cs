using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HLTStudio.GameCommons;

namespace HLTStudio.GameCustoms
{
	public static class SoundEffects
	{
		public static ASoundEffect Dummy = new ASoundEffect(@"General\muon1s.wav");

		public static ASoundEffect Save = new ASoundEffect(@"Freebie\ユーフルカ\save-01.wav");
		public static ASoundEffect Load = new ASoundEffect(@"Freebie\ユーフルカ\load.wav");
		public static ASoundEffect Buy = new ASoundEffect(@"Freebie\ユーフルカ\shop.wav");

		public static ASoundEffect EnemyDamaged = new ASoundEffect(@"Shoot_old_Resource\beetlepancake\shot003.wav");
		public static ASoundEffect EnemyKilled = new ASoundEffect(@"Freebie\小森平_Custom\兵器爆発\game_explosion6_wv_[50].wav");

		public static ASoundEffect PlayerShot = new ASoundEffect(@"Shoot_old_Resource\beetlepancake\shot004_wv_[5].wav");
		public static ASoundEffect Bomb = new ASoundEffect(@"Freebie\小森平\魔法ファンタジー\strange_beam.mp3");
		public static ASoundEffect PlayerCrashed = new ASoundEffect(@"Freebie\小森平\ゲームアニメ調\question.mp3");

		public static ASoundEffect 敵弾を吸収 = new ASoundEffect(@"Freebie\小森平\ゲームボタン音\poka01.mp3");

		public static ASoundEffect 学習開始 = new ASoundEffect(@"Freebie\小森平\ゲームボタン音\cat_like1a.mp3");
		public static ASoundEffect 学習完了 = new ASoundEffect(@"Freebie\小森平\ゲームボタン音\cat_like2b.mp3");

		public static ASoundEffect 対消滅 = new ASoundEffect(@"Freebie\小森平\ゲームボタン音\coin01.mp3");
		public static ASoundEffect アイテム取得 = new ASoundEffect(@"Freebie\小森平_Custom\ゲームボタン音\coin05_wv_[50].wav");
	}
}
