using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
using Box2D.XNA;
using Microsoft.Xna.Framework;

namespace cocos2dBox2DBreakOutDemo.Classes
{
    class BreakOutScene : CCScene
    {
        public BreakOutScene()
        {
            BreakOutLayer layer = BreakOutLayer.node();
            this.addChild(layer);
        }
        
    }

    class BreakOutLayer:CCLayer
    {
        public static double PTM_RATIO = 32.0;
        World world;
        Body groundBody;
        Fixture bottomFixture;
        Fixture ballFixture;
      
     
        public override bool init()
        {
            if (!base.init())
                return false;
            CCSize winSize = CCDirector.sharedDirector().getWinSize();
            CCLabelTTF title = CCLabelTTF.labelWithString(
                "Boxing", "Arial", 24);
            title.position = new CCPoint(winSize.width / 2, winSize.height - 50);
            this.addChild(title);

            Vector2 gravity = new Vector2(0.0f, 0.0f);
            bool doSleep = true;
            world = new World(gravity, doSleep);

            BodyDef groundBodyDef = new BodyDef();
            groundBodyDef.position = new Vector2(0, 0);
            groundBody = world.CreateBody(groundBodyDef);
            PolygonShape groundBox = new PolygonShape();
            FixtureDef boxShapeDef = new FixtureDef();
            boxShapeDef.shape = groundBox;
            groundBox.SetAsEdge(new Vector2(0, 0), new Vector2((float)(winSize.width / PTM_RATIO), 0));
            bottomFixture = groundBody.CreateFixture(boxShapeDef);
            groundBox.SetAsEdge(new Vector2(0, 0), new Vector2(0, (float)(winSize.height / PTM_RATIO)));
            groundBody.CreateFixture(boxShapeDef);
            groundBox.SetAsEdge(new Vector2(0, (float)(winSize.height / PTM_RATIO)),
                new Vector2((float)(winSize.width / PTM_RATIO), (float)(winSize.height / PTM_RATIO)));
            groundBody.CreateFixture(boxShapeDef);
            groundBox.SetAsEdge(new Vector2((float)(winSize.width / PTM_RATIO), (float)(winSize.height / PTM_RATIO)),
                new Vector2((float)(winSize.width / PTM_RATIO), 0));
            groundBody.CreateFixture(boxShapeDef);

            //定义小球

            CCSprite ball = CCSprite.spriteWithFile(@"images/Ball");
            ball.position = new CCPoint(100, 100);
            ball.tag = 1;
            this.addChild(ball);  





            BodyDef ballBodyDef = new BodyDef();
            ballBodyDef.type = BodyType.Dynamic;
            ballBodyDef.position = new Vector2((float)(100 / PTM_RATIO), (float)(100 / PTM_RATIO));
            ballBodyDef.userData = ball;
            Body ballBody = world.CreateBody(ballBodyDef);

            //Create circle shape  
            CircleShape circle = new CircleShape();
            circle._radius = (float)(26.0 / PTM_RATIO);

            //Create shape definition and add to body  
            FixtureDef ballShapeDef = new FixtureDef();
            ballShapeDef.shape = circle;
            ballShapeDef.density = 1.0f;
            ballShapeDef.friction = 0.0f;
            ballShapeDef.restitution = 1.0f;
            ballFixture = ballBody.CreateFixture(ballShapeDef);  


            Vector2 force = new Vector2(10, 10);
            ballBody.ApplyLinearImpulse(force, ballBodyDef.position);//Impules 冲力

            this.schedule(tick);

            

            return true;

        }

        public void tick(float dt)
        {
            world.Step(dt, 10, 10);
            for (Body b = world.GetBodyList(); b != null; b.GetNext())
            {
                if (b.GetUserData() != null)
                {
                    CCSprite sprite = (CCSprite)b.GetUserData();
                    sprite.position =
                        new CCPoint(
                            (float)(b.GetPosition().X * PTM_RATIO),
                            (float)(b.GetPosition().Y * PTM_RATIO));
                    sprite.rotation = -1 * MathHelper.ToDegrees(b.GetAngle());

                }
            }
        }
        public static new BreakOutLayer node()
        {
            BreakOutLayer layer = new BreakOutLayer();

            if (layer.init())
            {
                return layer;
            }

            return null;

        }
    }
}
