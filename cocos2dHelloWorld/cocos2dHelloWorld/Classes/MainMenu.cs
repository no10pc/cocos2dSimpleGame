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
            CCSprite background = CCSprite.spriteWithFile(@"images/sprites");
            background.position = new CCPoint(width / 2, height / 2);
            this.addChild(background);

            CCLabelTTF title = CCLabelTTF.labelWithString("Simple Game", "Arial", 24);
            title.Color = new ccColor3B(0, 255, 255);
            title.position = new CCPoint(width / 2, height - 50);
            this.addChild(title);

            CCMenuItemImage playItem = CCMenuItemImage.itemFromNormalImage(@"images/playButton", @"images/playButton", this, playCallback);

            CCMenuItemImage aboutItem = CCMenuItemImage.itemFromNormalImage(@"images/aboutButton", @"images/aboutButton", this, aboutCallback);


            CCMenu mainMenu = CCMenu.menuWithItems(playItem, aboutItem);
            mainMenu.alignItemsVerticallyWithPadding(15f);
            mainMenu.position = new CCPoint(100, height - 150);
            this.addChild(mainMenu);


            CCMenuItemImage exitItem = CCMenuItemImage.itemFromNormalImage(@"CloseNormal", @"CloseSelected", this, exitCallback);
            CCMenu exitMenu = CCMenu.menuWithItems(exitItem);
            exitMenu.position = new CCPoint(width - exitItem.contentSize.width / 2, exitItem.contentSize.height / 2);
            this.addChild(exitMenu);




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
