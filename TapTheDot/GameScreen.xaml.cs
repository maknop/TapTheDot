﻿using System;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace TapTheDot
{

    public partial class GameScreen : ContentPage
    {
        SharedResources s = new SharedResources();

        float randEnemy = 80;
        float fastRotation = 0;
        float rFastRotation = 0;
        float mediumRotation = 0;
        float rMediumRotation = 0;
        float slowRotation = 0;
        float rSlowRotation = 0;
        float slowestRotation = 0;
        float rSlowestRotation = 0;
        float currentRotation = 0;
        float x = 0;
        bool reverse = false;
        //int score;
        //int level;
        float positionCorrection = 0;
        //// create the paint for the filled circle
        SKPaint circleFill = new SKPaint
        {
            IsAntialias = true,
            Style = SKPaintStyle.Fill,
            Color = SKColors.White
        };
        // create the paint for the circle border
        SKPaint circleBorder = new SKPaint
        {
            IsAntialias = true,
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Blue,
            StrokeWidth = 5,
            StrokeCap = SKStrokeCap.Round
        };

        SKPaint playerLine = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Red,
            StrokeWidth = 8,
            StrokeCap = SKStrokeCap.Round,
            IsAntialias = true
            // the strokeWidth is subject to the scale and transforms
            // antialias makes it look nice
        };

        SKPaint enemy = new SKPaint
        {
            IsAntialias = true,
            Style = SKPaintStyle.Fill,
            Color = SKColors.Orange
        };

        public GameScreen()
        {
            InitializeComponent();

            //s.setLevel(level);
            //s.setScore(score);
            MainLabel.Text = "Score: " + s.getScore().ToString();
            LevelLabel.Text = "Level: " + s.getLevel().ToString();

            // In order for the player to continually move, we need to ensure the paint surface event handler is repeatedly executed
            // We want the timer to refresh 60 times per second, since that is the typical refresh rate of most monitors

            Device.StartTimer(TimeSpan.FromSeconds(1f / 144), () =>
              {
                  canvasView.InvalidateSurface();
                  return true;
              });
        }
        private float randValue()
        {
            Random cRand = new Random();
            return (float)cRand.NextDouble();
        }

        private void OnPainting(object sender, SKPaintSurfaceEventArgs e)
        {
            // we get the current surface from the event args
            SKSurface surface = e.Surface;
            // then we get the canvas that we can draw on
            SKCanvas canvas = surface.Canvas;
            // clear the canvas / view
            canvas.Clear(SKColors.CornflowerBlue);

            int width = e.Info.Width;
            int height = e.Info.Height;

            // Set transforms
            canvas.Translate(width / 2, height / 2);
            canvas.Scale(width / 400f);

            // draw the circle fill and border
            canvas.DrawCircle(0, 0, 100, circleFill);
            canvas.DrawCircle(0, 0, 150, circleBorder);


            float position = randValue();
            // Instantiate Date/Time
            DateTime dateTime = DateTime.Now;
            float milliseconds = dateTime.Millisecond;
            float seconds = dateTime.Second;

            // formula: (seconds % [# rotations per second] * [360 / # rotations per second]) + (milliseconds / (float)[1000 / (360 / # rotations per second)]
            // rotates once per second
            //float Rotation = milliseconds / (float)2.77777778;
            // rotates once every 2 seconds
            this.fastRotation = (seconds % 2 * 180) + (milliseconds / (float)5.5555556);
            //this.rFastRotation = -((seconds % 2 * 180) + (milliseconds / (float)5.5555556));
            // rotates once every 3 seconds
            this.mediumRotation = (seconds % 3 * 120) + (milliseconds / (float)8.333333333);
            //this.rMediumRotation = -((seconds % 3 * 120) + (milliseconds / (float)8.333333333));
            // rotates once every 4 seconds
            this.slowRotation = (seconds % 4 * 90) + (milliseconds / (float)11.11111111);
            //this.rSlowRotation = -((seconds % 4 * 90) + (milliseconds / (float)11.11111111));
            // rotates once every 5 seconds
            this.slowestRotation = (seconds % 5 * 72) + (milliseconds / (float)13.88888889);
            currentRotation = slowestRotation;
            //x = -((seconds % 5 * 72) + (milliseconds / (float)13.88888889)) + 360;
            //if (slowestRotation <= 180)
            //{
            //    //rSlowestRotation = (-((seconds % 5 * 72) + (milliseconds / (float)13.88888889))+360)-((-((seconds % 5 * 72) + (milliseconds / (float)13.88888889)) + 360)-slowestRotation);
            //    rSlowestRotation = x - (x + slowestRotation);
            //}
            //if (slowestRotation > 180)
            //{
            //    rSlowestRotation = x + (slowestRotation - x);
            //}

            DebugLabel.Text = "Enemy loc: " + randEnemy.ToString() + "Player loc: " + currentRotation;
            //if (level == 1 && reverse == false)
            //{
            //    currentRotation = slowestRotation;
            //}
            //if (level == 1 && reverse == true)
            //{
            //    currentRotation = rSlowestRotation;
            //}



            // We want to call the canvas.Save() method before the rotating the player line and then the canvas.Restore() method after
            canvas.Save();
            // Canvas.Rotate Degrees will rotate the canvas by the specified number of degrees. We want this number to count up to exactly 360 for a full circle
            canvas.RotateDegrees(currentRotation);
            // DrawLine will draw a line from from X1, Y1, to X2, Y2
            canvas.DrawLine(0, -100, 0, -150, playerLine);
            canvas.Restore();
            
            canvas.Save();
            canvas.RotateDegrees(randEnemy);
            canvas.DrawCircle(0, -125, 18, enemy);
            canvas.Restore();

        }
        private void Button_ClickedBack(object sender, EventArgs e)
        {
            //s.setScore(score);
            //s.setLevel(level);
            //s.incrementLevel();
            //s.incrementScore();
            App.Current.MainPage = new HomePage();
        }


        private void Button_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Notification", "Do you want to save this item?", "Save", "Don't Save");
        }

        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            _ = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = Color.Red.ToSKColor(),
                StrokeWidth = 25
            };

        }
        private float randMovement()
        {
            float enemyMovement = randValue();
            return enemyMovement;
        }


        private void Button_Clicked_2(object sender, EventArgs e)
        {
            if ((currentRotation%360 )+ 27 > randEnemy && (currentRotation%360) - 27 < randEnemy)
            {
         
                randEnemy = randMovement() * 360;
                //score += 1;
                //s.setScore(score);
                s.incrementScore();
                MainLabel.Text = "Score: " + s.getScore().ToString();
                LevelLabel.Text = "Level: " + s.getLevel().ToString();
                if (s.getScore() % 2 == 0) // % 5 == 4
                {
                    //level += 1;
                    //s.setLevel(level);
                    s.incrementLevel();
                }
                //if (score % 2 == 1)
                //{
                //    reverse = true;
                //}
                //if (score % 2 == 0)
                //{
                //    reverse = false;
                //}

            }

        }
    }

}
