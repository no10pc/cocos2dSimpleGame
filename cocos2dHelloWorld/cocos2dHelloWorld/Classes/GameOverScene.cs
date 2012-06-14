using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
using Microsoft.Phone.Shell;
namespace cocos2dSimpleGame.Classes
{
    class GameOverScene : CCScene
    {
        public CCLabelTTF label;
        public GameOverScene()
        {

        }
        public GameOverScene(bool isWin)
        {
            CCLayerColor colorlayer = CCLayerColor.layerWithColor(new ccColor4B(255, 255, 255, 255));
            this.addChild(colorlayer);
            CCSize winSize = CCDirector.sharedDirector().getWinSize();

            string msg = isWin ? "YOU WIN" : "YOU LOSE";


            label = CCLabelTTF.labelWithString(msg, "Arial", 32);
            label.Color = new ccColor3B(0, 0, 0);
            label.position = new CCPoint(winSize.width / 2, winSize.height / 2);
            this.addChild(label);

            var itemReplay = CCMenuItemImage.itemFromNormalImage(@"images/reload", @"images/reload", this, replay);
            var itemMainMenu = CCMenuItemImage.itemFromNormalImage(@"images/mainmenu", @"images/mainmenu", this, mainmenu);
            var itemNextLevel = CCMenuItemImage.itemFromNormalImage(@"images/nextlevel ", @"images/nextlevel", this, nextlevel);
            if (!isWin)
            {
                itemNextLevel.visible = false;
            }
            var menu = CCMenu.menuWithItems(itemReplay, itemMainMenu, itemNextLevel);
            menu.alignItemsHorizontally();
            menu.position = new CCPoint(
                winSize.width / 2, winSize.height / 2 - 100);
            this.addChild(menu);

            //this.runAction(CCSequence.actions(
            //    CCDelayTime.actionWithDuration(3), CCCallFunc.actionWithTarget(this, gameOverDone)));
        }
        void nextLevel(object sender)
        {
            //http://blog.csdn.net/fengyun1989/article/details/7486505
            GamePlayScene pScene;
            if (PhoneApplicationService.Current.State.ContainsKey("PlayScene"))
            {

            }
        }

        void gameOverDone()
        {
            CCScene pScene = CCScene.node();
            pScene.addChild(MainMenu.node());
            CCDirector.sharedDirector().replaceScene(pScene);
        }
    }
}
