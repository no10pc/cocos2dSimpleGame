using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

using CocosDenshion;
using Microsoft.Xna.Framework;
using Microsoft.Phone.Shell;

namespace cocos2dSimpleGame.Classes
{
    public class GamePlayScene : CCScene
    {
        //public override void onEnter()
        //{
        //    base.onEnter();
        //    CCLayerColor colorLayer = CCLayerColor.layerWithColor(new ccColor4B(255, 255, 255, 0));
        //    this.addChild(colorLayer);
        //    this.addChild(GamePlayLayer.node());
        //}
        public GamePlayScene()
        {
            CCLayerColor colorLayer = CCLayerColor.layerWithColor(new ccColor4B(255, 255, 255, 255));
            this.addChild(colorLayer);
            GamePlayLayer player = (GamePlayLayer)GamePlayLayer.node();
            player.tag = 3;
            this.addChild(player);
            PhoneApplicationService.Current.State["PlayScene"] = this;
        }
    }
    class GamePlayLayer : CCLayer
    {
        List<CCSprite> _targets;
        List<CCSprite> _projectiles;
        int projectilesDestroyed = 0;
        int quitProjectiles = 0;
        public int count = 0;
        public CCLabelTTF title;

        public CCSprite player;
        CCSprite target;

        Level level = new Level(1);
        int life = 40;
        CCLabelTTF notic;


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


            player = CCSprite.spriteWithFile(@"images/Player2");
            player.position = new CCPoint(20, screenHeight / 2);
            this.addChild(player);


            title = CCLabelTTF.labelWithString(count.ToString("0000"), "Arial", 24);
            title.Color = new ccColor3B(0, 255, 255);
            title.position = new CCPoint(50, 24);
            this.addChild(title);

            string msg = String.Format("Count:{0},life:{1},Level:{2}", projectilesDestroyed, life, level.level);
            notic = CCLabelTTF.labelWithString(msg, "Arial", 24);
            notic.position = new CCPoint(notic.contentSize.width / 2, screenHeight - notic.contentSize.height / 2);
            addChild(notic);


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
            //string number = random.Next(1, 15).ToString("00");
            //if (int.Parse(number) / 2 == Convert.ToDouble(number) / 2)
            //{
            //    target = CCSprite.spriteWithFile(@"cars/sonice_cars_" + number);
            //}
            //else
            //{
            //    target = CCSprite.spriteWithFile(@"cars/lights_" + number);
            //}
            Monster target = null;
            //if (random.Next() % 2 == 0)
            //{
            //    target = WeakAndFastMonster.monster();
            //}
            //else
            //{
            //    target = StrongAndSlowMonster.monster();
            //}
            target = level.GetMonster();

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
            //float minDuration = 4.0f;
            //float maxDuration = 7.0f;
            float minDuration = target.minMoveDuration;
            float maxDuration = target.maxMoveDuration;
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
                life--;
                string msg = string.Format("Count:{0},life:{1},Level:{2}", projectilesDestroyed, life, level.level);
                notic.setString(msg);
                if (life <= 0)
                {
                    GameOverScene pScene = new GameOverScene(false);
                    CCDirector.sharedDirector().replaceScene(pScene);

                }
            }
            else if (sprite.tag == 2)
            {
                _projectiles.Remove(sprite);
            }
            this.removeChild(sprite, true);

        }
        CCSprite nextProjectile = null;
        public override void ccTouchesEnded(List<CCTouch> touches, CCEvent event_)
        {

            if (nextProjectile != null)
                return;

            CCTouch touch = touches.FirstOrDefault();
            CCPoint location = touch.locationInView(touch.view());
            location = CCDirector.sharedDirector().convertToGL(location);

            //set up initial location of projectile  
            CCSize winSize = CCDirector.sharedDirector().getWinSize();
            //CCSprite projectile = CCSprite.spriteWithFile(@"images/Projectile");  
            nextProjectile = CCSprite.spriteWithFile(@"images/Projectile2");
            nextProjectile.position = new CCPoint(20, winSize.height / 2);

            //Determine offset of location to projectile  

            float offX = location.x - nextProjectile.position.x;
            float offY = location.y - nextProjectile.position.y;

            //Bail out if we are shooting or backwards  
            if (offX <= 0)
            {
                return;
            }



            //Determine where we wish to shoot the projectile to  
            float realX = winSize.width + nextProjectile.contentSize.width / 2;
            float ratio = offY / offX;
            float realY = realX * ratio + nextProjectile.position.y;
            CCPoint realDest = new CCPoint(realX, realY);

            //Determine the length of how far we're shooting  
            float offRealX = realX - nextProjectile.position.x;
            float offRealY = realY - nextProjectile.position.y;
            float length = (float)Math.Sqrt(offRealX * offRealX + offRealY * offRealY);
            float velocity = 480 / 1;//480pixls/lsec  
            float realMoveDuration = length / velocity;


            //Determine angle to face  
            float angleRadians = (float)Math.Atan(offRealY / offRealX);
            float angleDegrees = MathHelper.ToDegrees(angleRadians);
            float cocosAngle = -1 * angleDegrees;

            float rotateSpeed = (float)(0.5 / Math.PI);//Would take 0.5 seconds to rotate 0.5 radians ,or half a circle  
            float rotateDuration = Math.Abs(angleRadians * rotateSpeed);
            player.runAction(CCSequence.actions(CCRotateTo.actionWithDuration(rotateDuration, cocosAngle), CCCallFunc.actionWithTarget(this, finishShoot)));
            //Move projectile to actual endpoint  
            nextProjectile.runAction(CCSequence.actions(CCMoveTo.actionWithDuration(realMoveDuration, realDest),
                CCCallFuncN.actionWithTarget(this, spriteMoveFinished)));
            nextProjectile.tag = 2;

        }
        void finishShoot()
        {
            this.addChild(nextProjectile);
            _projectiles.Add(nextProjectile);
            SimpleAudioEngine.sharedEngine().playEffect(@"sounds/biubiu");
            nextProjectile = null;
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
                bool monsterHit = false;
                foreach (CCSprite target in _targets)
                {
                    CCRect targetRect = new CCRect(
                        target.position.x - target.contentSize.width / 2,
                        target.position.y - target.contentSize.height / 2,
                        target.contentSize.width,
                        target.contentSize.height);
                    if (CCRect.CCRectIntersetsRect(projectileRect, targetRect))
                    {
                        monsterHit = true;
                        Monster monster = (Monster)target;
                        monster.hp--;
                        if (monster.hp <= 0)
                        {
                            targetToDelete.Add(target);
                        }
                        break;
                    }
                }
                foreach (CCSprite target in targetToDelete)
                {
                    _targets.Remove(target);
                    projectilesDestroyed++;
                    string msg = String.Format("Count:{0},life:{1},Level:{2}", projectilesDestroyed, life, level.level);
                    notic.setString(msg);
                    if (projectilesDestroyed >= level.levelCount)
                    {
                        GameOverScene pScene = new GameOverScene(true);
                        CCDirector.sharedDirector().replaceScene(pScene);
                    }
                    this.removeChild(target, true);
                }

                if (monsterHit)
                {
                    projectilesToDelete.Add(projectile);
                    SimpleAudioEngine.sharedEngine().playEffect(@"sounds/explosion");
                }


            }
            foreach (CCSprite projectile in projectilesToDelete)
            {
                _projectiles.Remove(projectile);
                quitProjectiles++;
                if (quitProjectiles > 10)
                {
                    //GameOverScene pScene = new GameOverScene("You Lost");
                    //CCDirector.sharedDirector().replaceScene(pScene);
                }
                this.removeChild(projectile, true);
            }
            projectilesToDelete.Clear();
        }

        public void Reset(bool replay)
        {
            foreach (var item in _targets)
            {
                this.removeChild(item, true);
            }
            foreach (var item in _projectiles)
            {
                this.removeChild(item, true);
            }
            _targets.Clear();
            _projectiles.Clear();
            projectilesDestroyed = 0;
            nextProjectile = null;
            if (replay)
            {
                life = 40;
            }
            else
            {
                level.NextLevel();
            }
            this.schedule(gameLogic, 1.0f);
            this.schedule(updates);
            string msg = String.Format("Count:{0},life:{1},Level:{2}", projectilesDestroyed, life, level.level);
            notic.setString(msg);
        }
    }
}
