using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
namespace cocos2dSimpleGame.Classes
{
    class MainMenu:CCLayer
    {
        public override bool init()
        {
            if (!base.init())
            {
                return base.init();
            }

            CCDirector.sharedDirector().deviceOrientation = ccDeviceOrientation.CCDeviceOrientationLandscapeLeft;
            this.m_bIsTouchEnabled = true;
            var width = CCDirector.sharedDirector().getWinSize().width;
            var height = CCDirector.sharedDirector().getWinSize().height;
            CCSprite background = CCSprite.spriteWithFile(@"images/background");
            background.position = new CCPoint(width / 2, height / 2);
            this.addChild(background);

            //var itemReplay = CCMenuItemImage.itemFromNormalImage(@"images/reload", @"images/reload", this, replay);
            //var itemMainMenu = CCMenuItemImage.itemFromNormalImage(@"images/mainmenu", @"images/mainmenu", this, mainmenu);
            //var itemNextLevel = CCMenuItemImage.itemFromNormalImage(@"images/nextlevel ", @"images/nextlevel", this, nextlevel);

            CCMenuItemImage playItem = CCMenuItemImage.itemFromNormalImage(@"images/home_64", @"images/home_64", this, playCallback);

            CCMenuItemImage aboutItem = CCMenuItemImage.itemFromNormalImage(@"images/gear_64", @"images/gear_64", this, aboutCallback);

            CCMenuItemImage exitItem = CCMenuItemImage.itemFromNormalImage(@"images/delete_64", @"images/delete_64", this, exitCallback);

            CCMenu mainMenu = CCMenu.menuWithItems(playItem, aboutItem,exitItem);
            mainMenu.alignItemsHorizontallyWithPadding(100f);
            mainMenu.position = new CCPoint(400, 100);
            this.addChild(mainMenu);


          
        
          




            return true;
        }
        public static new CCLayer node()
        {
            MainMenu ret = new MainMenu();

            if (ret.init())
            {
                return ret;
            }
            else
            {
                ret = null;
            }

            return ret;
        }
        void playCallback(object sender)
        {
            GamePlayScene pScene = new GamePlayScene();
            CCDirector.sharedDirector().pushScene(pScene);
        }
        void aboutCallback(object sender)
        {
            AboutScene pScene = new AboutScene();
            CCDirector.sharedDirector().replaceScene(pScene);

        }
        void exitCallback(object sender)
        {
            CCDirector.sharedDirector().end();
            CCApplication.sharedApplication().Game.Exit();
        }
    }
}
