using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
using CocosDenshion;

namespace cocos2dSimpleGame.Classes
{
    public class GamePlayScreen : CCScene
    {
        public override void onEnter()
        {
            base.onEnter();
            CCLayerColor colorLayer = CCLayerColor.layerWithColor(
                new ccColor4B(255, 255, 255, 0));
            this.addChild(colorLayer);
            this.addChild(GamePlayLayer.node());
        }
    }
    class GamePlayLayer : CCLayer
    {
        List<CCSprite> _targets;
        List<CCSprite> _projectiles;
        int projectileDescroyed = 0;
        int quitProjectiles = 0;
        public int count = 0;
        public CCLabelTTF title;  
        public override bool init()
        {
            SimpleAudioEngine.sharedEngine().playBackgroundMusic(@"sounds/background");
            _projectiles = new List<CCSprite>();
            _targets = new List<CCSprite>();

            if (!base.init())
            {
                return false;
            }
            CCDirector.sharedDirector().deviceOrientation = ccDeviceOrientation.CCDeviceOrientationLandscapeLeft;
            this.m_bIsTouchEnabled = true;
            var screenWidth = CCDirector.sharedDirector().getWinSize().width;
            var screenHeight = CCDirector.sharedDirector().getWinSize().height;


            CCSprite player = CCSprite.spriteWithFile(@"images/Player");
            player.position = new CCPoint(20, screenHeight / 2);
            this.addChild(player);


            title = CCLabelTTF.labelWithString(count.ToString("0000"), "Arial", 24);
            title.Color = new ccColor3B(0, 255, 255);
            title.position = new CCPoint(50, 24);
            this.addChild(title);



            this.schedule(gameLogic, 1.0f);
            this.schedule(updates);
            return true;
        }
        void gameLogic(float dt)
        {
            this.addTarget();
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
        /// <summary>
        /// 生成敌人
        /// </summary>
        private void addTarget()
        {
            Random random = new Random();
            var screenWidth = CCDirector.sharedDirector().getWinSize().width;
            var screenHeight = CCDirector.sharedDirector().getWinSize().height;
            string number = random.Next(1, 15).ToString("00");
            CCSprite target = CCSprite.spriteWithFile(@"cars/sonice_cars_" + number);

            var minY = target.contentSize.height / 2;
            var maxY = screenHeight - target.contentSize.height / 2;
            float rangeY = maxY - minY;
            float actualY = (random.Next() % rangeY) + minY;

            //create the target slightly off-screen along the right edge;  
            //and along a random position along the Y axis as calculated above  
            target.position = new CCPoint(screenWidth + screenWidth / 2, actualY);
            //
            target.tag = 1;
            _targets.Add(target);

            this.addChild(target);


            //Determine speed of the target  
            float minDuration = 4.0f;
            float maxDuration = 7.0f;
            float rangeDuration = maxDuration - minDuration;
            float actualDuration = random.Next() % rangeDuration + minDuration;

            //Create the actions  
            var actionMove = CCMoveTo.actionWithDuration(actualDuration, new CCPoint(-target.contentSize.width / 2, actualY));

            var actionMoveDone = CCCallFuncN.actionWithTarget(this, spriteMoveFinished);
            target.runAction(CCSequence.actions(actionMove, actionMoveDone));



        }
        /// <summary>
        /// 移除敌人
        /// </summary>
        /// <param name="sender"></param>
        void spriteMoveFinished(object sender)
        {

            CCSprite sprite = (CCSprite)sender;
            if (sprite.tag == 1)
            {
                _targets.Remove(sprite);
            }
            else if (sprite.tag == 2)
            {
                _projectiles.Remove(sprite);
            }
            this.removeChild(sprite, true);

        }

        public override void ccTouchesEnded(List<CCTouch> touches, CCEvent event_)
        {
            CCTouch touch = touches.FirstOrDefault();
            CCPoint location = touch.locationInView(touch.view());
            location = CCDirector.sharedDirector().convertToGL(location);

            CCSize winSize = CCDirector.sharedDirector().getWinSize();
            CCSprite projectile = CCSprite.spriteWithFile(@"images/Projectile");
            projectile.position = new CCPoint(20, winSize.height / 2);

            float offx = location.x - projectile.position.x;
            float offy = location.y - projectile.position.y;

            if (offx < 0)
            {
                return;
            }

            projectile.tag = 2;
            _projectiles.Add(projectile);
            this.addChild(projectile);

            float realX = winSize.width + projectile.contentSize.width / 2;
            float ratio = offy / offx;
            float realY = realX * ratio + projectile.position.y;
            CCPoint realDest = new CCPoint(realX, realY);

            float offRealX = realX - projectile.position.x;
            float offRealY = realY - projectile.position.y;
            float length = (float)Math.Sqrt(offRealX * offRealX + offRealY * offRealY);
            float velocity = 480 / 1;
            float realmoveDuration = length / velocity;
            //Action  MoveTo
            SimpleAudioEngine.sharedEngine().playEffect(@"sounds/biubiu");
            projectile.runAction(CCSequence.actions(CCMoveTo.actionWithDuration(realmoveDuration, realDest), CCCallFuncN.actionWithTarget(this, spriteMoveFinished)));

        }

        public void updates(float dt)
        {
            List<CCSprite> projectilesToDelete = new List<CCSprite>();
            List<CCSprite> targetToDelete = new List<CCSprite>();

            for (int i = 0; i < _projectiles.Count; i++)
            {
                CCSprite projectile = _projectiles[i];
                CCRect projectileRect = new CCRect(
                    projectile.position.x - projectile.contentSize.width / 2,
                    projectile.position.y - projectile.contentSize.height / 2,
                    projectile.contentSize.width,
                    projectile.contentSize.height);
                foreach (CCSprite target in _targets)
                {
                    CCRect targetRect = new CCRect(
                        target.position.x - target.contentSize.width / 2,
                        target.position.y - target.contentSize.height / 2,
                        target.contentSize.width,
                        target.contentSize.height);
                    if (CCRect.CCRectIntersetsRect(projectileRect, targetRect))
                    {
                        SimpleAudioEngine.sharedEngine().playEffect(@"sounds/boom");
                        targetToDelete.Add(target);
                    }
                }
                foreach (CCSprite target in targetToDelete)
                {
                    _targets.Remove(target);
                    projectileDescroyed++;
                    title.setString(projectileDescroyed.ToString("0000"));
                    if (projectileDescroyed > 10)
                    {
                        GameOverScene pScene = new GameOverScene("You Win");
                        CCDirector.sharedDirector().replaceScene(pScene);
                    }
                    this.removeChild(target, true);
                }
                if (targetToDelete.Count > 0)
                {
                    projectilesToDelete.Add(projectile);
                }

            }
            foreach (CCSprite projectile in projectilesToDelete)
            {
                _projectiles.Remove(projectile);
                quitProjectiles++;
                if (quitProjectiles > 10)
                {
                    GameOverScene pScene = new GameOverScene("You Lost");
                    CCDirector.sharedDirector().replaceScene(pScene);
                }
                this.removeChild(projectile, true);
            }
            projectilesToDelete.Clear();
        }
    }
}
