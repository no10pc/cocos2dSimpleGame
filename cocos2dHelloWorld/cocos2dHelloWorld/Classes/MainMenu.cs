using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
namespace cocos2dSimpleGame
{
   public class MainMenu:CCLayer
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
            GamePlayScreen pScene = new GamePlayScreen();
            CCDirector.sharedDirector().pushScene(pScene);
        }
        void aboutCallback(object sender)
        {
        }
        void exitCallback(object sender)
        {
            CCDirector.sharedDirector().end();
            CCApplication.sharedApplication().Game.Exit();
        }
    }
}
