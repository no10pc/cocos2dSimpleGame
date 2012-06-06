using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace cocos2dSimpleGame
{
    public class GamePlayScreen : CCScene
    {
        public override void onEnter()
        {
            base.onEnter();
            CCLayerColor colorLayer = CCLayerColor.layerWithColor(
                new ccColor4B(255, 255, 255, 255));
            this.addChild(colorLayer);
            this.addChild(GamePlayLayer.node());
        }
    }
    class GamePlayLayer : CCLayer
    {
        public override bool init()
        {
            if (!base.init())
            {
                return false;
            }
            CCDirector.sharedDirector().deviceOrientation = ccDeviceOrientation.CCDeviceOrientationLandscapeLeft;
            this.m_bIsTouchEnabled = true;
            var screenWidth = CCDirector.sharedDirector().getWinSize().width;
            var screenHeight = CCDirector.sharedDirector().getWinSize().height;


            CCSprite player = CCSprite.spriteWithFile(@"images/player");
            player.position = new CCPoint(player.contentSize.width / 2, screenHeight / 2);
            this.addChild(player);

            this.schedule(gameLogic, 1.0f);  
            return true;
        }
        public static new CCLayer node()
        {
            GamePlayLayer screen = new GamePlayLayer();
            if (screen.init())
            {
                return screen;
            }
            else
            {
                screen = null;
            }
            return screen;

        }
        private void addTarget()
        {
            Random random = new Random();
            var screenWidth = CCDirector.sharedDirector().getWinSize().width;
            var screenHeight = CCDirector.sharedDirector().getWinSize().height;
            CCSprite target = CCSprite.spriteWithFile(@"images/Target");
            var minY = target.contentSize.height / 2;
            var maxY = screenHeight - target.contentSize.height / 2;
            float rangeY = maxY - minY;
            float actualY = (random.Next() % rangeY) + minY;

            //create the target slightly off-screen along the right edge;  
            //and along a random position along the Y axis as calculated above  
            target.position = new CCPoint(screenWidth + screenWidth / 2, actualY);
            this.addChild(target);

            //Determine speed of the target  
            float minDuration = 2.0f;
            float maxDuration = 4.0f;
            float rangeDuration = maxDuration - minDuration;
            float actualDuration = random.Next() % rangeDuration + minDuration;

            //Create the actions  
            var actionMove = CCMoveTo.actionWithDuration(actualDuration, new CCPoint(-target.contentSize.width / 2, actualY));
            var actionMoveDone = CCCallFuncN.actionWithTarget(this, spriteMoveFinished);
            target.runAction(CCSequence.actions(actionMove, actionMoveDone));

        }

        void spriteMoveFinished(object sender)
        {
            CCSprite sprite = (CCSprite)sender;
            this.removeChild(sprite, true);
        }
        void gameLogic(float dt)
        {
            this.addTarget();
        }  
    }
}
