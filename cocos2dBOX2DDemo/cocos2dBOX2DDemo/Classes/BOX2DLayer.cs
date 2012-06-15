using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
using Box2D.XNA;
using Microsoft.Xna.Framework;
namespace cocos2dBOX2DDemo.Classes
{
    class BOX2DLayer : CCLayer
    {
        public static double PTM_RATIO = 32.0;
        World world;
        Body body;
        CCSprite ball;
        CCLabelTTF title = null;
        public override bool init()
        {
            if (!base.init())
                return false;
            if (!base.init())
            {
                return false;
            }


            //CCSize winSize = CCDirector.sharedDirector().getWinSize();
            //title = CCLabelTTF.labelWithString("FootBall", "Arial", 24);
            //title.position = new CCPoint(winSize.width / 2, winSize.height  - 50);
            //this.addChild(title,1);
            //ball = CCSprite.spriteWithFile(@"images/ball");
            //ball.position = new CCPoint(100, 300);
            //ball.addChild(ball);
            //Vector2 gravity = new Vector2(0.0f, -30f);
            //bool doSleep = true;
            //world = new World(gravity, doSleep);


            CCSize winSize = CCDirector.sharedDirector().getWinSize();
            title = CCLabelTTF.labelWithString("FootBall", "Arial", 24);
            title.position = new CCPoint(winSize.width / 2, winSize.height - 50);
            this.addChild(title, 1);
            ball = CCSprite.spriteWithFile(@"images/ball");
            ball.position = new CCPoint(100, 300);
            this.addChild(ball);
            Vector2 gravity = new Vector2(0.0f, -30.0f);
            bool doSleep = true;
            world = new World(gravity, doSleep);






            /////////////////////////
            BodyDef groundBodyDef = new BodyDef();
            groundBodyDef.position = new Vector2(0, 0);
            Body groundBody = world.CreateBody(groundBodyDef);
            PolygonShape groundBox = new PolygonShape();
            FixtureDef boxShapeDef = new FixtureDef();
            boxShapeDef.shape = groundBox;

            groundBox.SetAsEdge(new Vector2(0, 0), new Vector2((float)(winSize.width / PTM_RATIO), 0));
            groundBody.CreateFixture(boxShapeDef);
            groundBox.SetAsEdge(new Vector2(0, 0), new Vector2(0, (float)(winSize.height / PTM_RATIO)));
            groundBody.CreateFixture(boxShapeDef);
            groundBox.SetAsEdge(new Vector2(0, (float)(winSize.height / PTM_RATIO)),
                new Vector2((float)(winSize.width / PTM_RATIO), (float)(winSize.height / PTM_RATIO)));
            groundBody.CreateFixture(boxShapeDef);
            groundBox.SetAsEdge(new Vector2((float)(winSize.width / PTM_RATIO), (float)(winSize.height / PTM_RATIO)),
                new Vector2((float)(winSize.width / PTM_RATIO), 0));
            groundBody.CreateFixture(boxShapeDef);

            BodyDef ballBodyDef = new BodyDef();
            ballBodyDef.type = BodyType.Dynamic;
            ballBodyDef.position = new Vector2(
                (float)(100 / PTM_RATIO),
                (float)(300 / PTM_RATIO));
            ballBodyDef.userData = ball;
            body = world.CreateBody(ballBodyDef);

            CircleShape circle = new CircleShape();
            circle._radius = (float)(26.0 / PTM_RATIO);

            FixtureDef ballShapeDef = new FixtureDef();
            ballShapeDef.shape = circle;
            ballShapeDef.density = 1.0f;
            ballShapeDef.friction = 0.0f;
            ballShapeDef.restitution = 1.0f;
            body.CreateFixture(ballShapeDef);

            this.schedule(tick);
            return true;

        }

        public void tick(float dt)
        {
            world.Step(dt, 10, 10);
            for (Body b = world.GetBodyList(); b != null; b = b.GetNext())
            {
                if (b.GetUserData() != null)
                {
                    CCSprite ballData = (CCSprite)b.GetUserData();
                    ballData.position = new CCPoint(
                        (float)(b.GetPosition().X * PTM_RATIO),
                        (float)(b.GetPosition().Y * PTM_RATIO));
                    ballData.rotation = -1 * MathHelper.ToDegrees(b.GetAngle());
                                        title.setString(string.Format("X:{0}Y:{1}",ballData.position.x,ballData.position.y));

                }
            }
        }
        public static new CCLayer node()
        {
            BOX2DLayer layer = new BOX2DLayer();
            if (layer.init())
            {
                return layer;
            }
            else
                layer = null;
            return layer;
        }

    }
}
