using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2dSimpleGame.Classes
{
    class Level
    {

        int _level;
        int _levelCount;

        public int levelCount { get { return _levelCount; } }
        public int level { get { return _level; } }
        public Level(int l)
        {
            if (l <= 0 || l > 7)
                _level = 1;
            else
                _level = l;
            _levelCount = GetLevelCount(_level);
        }
        private int GetLevelCount(int level)
        {
            switch (level)
            {
                case 1:
                    return 10;
                case 2:
                    return 10;
                case 3:
                    return 35;
                case 4: return 50;
                case 5: return 55;
                case 6: return 60;
                case 7: return 65;
                default:
                    return 30;
            }  
        }
        public void NextLevel()
        {
            _level++;
            if (_level > 7)
            {
                _level = 1;
            }
            _levelCount = GetLevelCount(_level);

        }
        public Monster GetMonster()
        {
            Monster monster;
            Random random = new Random();
            switch (level)
            {
                case 1: monster = WeakAndFastMonster.monster(1, 5, 8); break;
                case 2: monster = WeakAndFastMonster.monster(1, 4, 7); break;
                case 3: monster = WeakAndFastMonster.monster(1, 3, 5); break;
                case 4:
                    {
                        if (random.Next() % 7 == 0)
                            monster = StrongAndSlowMonster.monster(3, 6, 12);
                        else
                            monster = WeakAndFastMonster.monster(1, 3, 6);
                        break;
                    }
                case 5:
                    {
                        if (random.Next() % 5 == 0)
                            monster = StrongAndSlowMonster.monster(3, 6, 12);
                        else
                            monster = WeakAndFastMonster.monster(1, 3, 6);
                        break;
                    }
                case 6:
                    {
                        if (random.Next() % 4 == 0)
                            monster = StrongAndSlowMonster.monster(3, 6, 12);
                        else
                            monster = WeakAndFastMonster.monster(1, 2, 6);
                        break;
                    }
                case 7:
                    {
                        if (random.Next() % 3 == 0)
                            monster = StrongAndSlowMonster.monster(3, 6, 12);
                        else
                            monster = WeakAndFastMonster.monster(1, 3, 6);
                        break;
                    }
                default:
                    monster = WeakAndFastMonster.monster(1, 3, 7); break;
            }
            return monster;
        }
    }
}
