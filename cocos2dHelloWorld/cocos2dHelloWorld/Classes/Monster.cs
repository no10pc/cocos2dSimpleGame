using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
namespace cocos2dSimpleGame.Classes
{
    class Monster : CCSprite
    {
        public int hp { get; set; }
        public int maxMoveDuration { get; set; }
        public int minMoveDuration { get; set; }
    }
    class WeakAndFastMonster : Monster
    {
        public static WeakAndFastMonster monster(int _hp, int _minMoveDuration, int _maxMoveDuration)
        {
            WeakAndFastMonster monster = new WeakAndFastMonster();

            if (monster.initWithFile(@"cars/Target"))
            {
                monster.hp = _hp;
                monster.minMoveDuration =_minMoveDuration;
                monster.maxMoveDuration = _maxMoveDuration;
            }

            return monster;
        }
    }
    class StrongAndSlowMonster : Monster
    {
        public static StrongAndSlowMonster monster(int _hp, int _minMoveDuration, int _maxMoveDuration)
        {
            StrongAndSlowMonster monster = new StrongAndSlowMonster();

            if (monster.initWithFile(@"cars/Target2"))
            {
                monster.hp = _hp;
                monster.minMoveDuration = _minMoveDuration;
                monster.maxMoveDuration = _maxMoveDuration;
            }
            return monster;
        }
    }
}
