using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace PalashDynamics.Animations
{
    public enum RotationDirection
    {
        LeftToRight,
        RightToLeft,
        TopToBottom,
        BottomToTop
    }

    public enum RotationType
    {
        Forward,
        Backward
    }

    public class RotationData
    {
        public double FromDegrees { get; set; }
        public double MidDegrees { get; set; }
        public double ToDegrees { get; set; }
        public string RotationProperty { get; set; }
        public PlaneProjection PlaneProjection { get; set; }
        public Duration AnimationDuration { get; set; }
    }

    public class SwivelAnimation
    {

        public SwivelAnimation(UIElement FrontElement, UIElement BackElement, RotationDirection Rotation, Duration Duration)
        {
            this.FrontElement = FrontElement;
            this.BackElement = BackElement;
            this.Rotation = Rotation;
            this.Duration = Duration;
        }
        private readonly Storyboard frontToBackStoryboard = new Storyboard();
        private readonly Storyboard backToFrontStoryboard = new Storyboard();
        #region Front element

        public UIElement FrontElement { get; set; }

        #endregion

        #region Back element

        public UIElement BackElement { get; set; }

        #endregion

        #region Duration

        public Duration Duration { get; set; }

        #endregion

        #region Rotation Direction

        public RotationDirection Rotation { get; set; }

        #endregion

        private RotationType CurrentStatus = RotationType.Backward;

        public void Invoke(RotationType RotationType)
        {
            if (FrontElement == null || BackElement == null) return;

            if (FrontElement.Projection == null || BackElement.Projection == null)
            {
                FrontElement.Projection = new PlaneProjection();
                FrontElement.RenderTransformOrigin = new Point(.5, .5);
                FrontElement.Visibility = Visibility.Visible;

                BackElement.Projection = new PlaneProjection { CenterOfRotationY = .5, RotationY = 180.0 }; //, CenterOfRotationZ = this.CenterOfRotationZ };
                BackElement.RenderTransformOrigin = new Point(.5, .5);
                BackElement.Visibility = Visibility.Collapsed;

                RotationData showBackRotation = null;
                RotationData hideFrontRotation = null;
                RotationData showFrontRotation = null;
                RotationData hideBackRotation = null;

                var frontPP = new PlaneProjection(); // { CenterOfRotationZ = this.CenterOfRotationZ };
                var backPP = new PlaneProjection(); // { CenterOfRotationZ = this.CenterOfRotationZ };

                switch (Rotation)
                {
                    case RotationDirection.LeftToRight:
                        backPP.CenterOfRotationY = frontPP.CenterOfRotationY = 0.5;
                        showBackRotation = new RotationData { FromDegrees = 180.0, MidDegrees = 90.0, ToDegrees = 0.0, RotationProperty = "RotationY", PlaneProjection = backPP, AnimationDuration = Duration };
                        hideFrontRotation = new RotationData { FromDegrees = 0.0, MidDegrees = -90.0, ToDegrees = -180.0, RotationProperty = "RotationY", PlaneProjection = frontPP, AnimationDuration = Duration };
                        showFrontRotation = new RotationData { FromDegrees = -180.0, MidDegrees = -90.0, ToDegrees = 0.0, RotationProperty = "RotationY", PlaneProjection = frontPP, AnimationDuration = Duration };
                        hideBackRotation = new RotationData { FromDegrees = 0.0, MidDegrees = 90.0, ToDegrees = 180.0, RotationProperty = "RotationY", PlaneProjection = backPP, AnimationDuration = Duration };
                        break;
                    case RotationDirection.RightToLeft:
                        backPP.CenterOfRotationY = frontPP.CenterOfRotationY = 0.5;
                        showBackRotation = new RotationData { FromDegrees = -180.0, MidDegrees = -90.0, ToDegrees = 0.0, RotationProperty = "RotationY", PlaneProjection = backPP, AnimationDuration = Duration };
                        hideFrontRotation = new RotationData { FromDegrees = 0.0, MidDegrees = 90.0, ToDegrees = 180.0, RotationProperty = "RotationY", PlaneProjection = frontPP, AnimationDuration = Duration };
                        showFrontRotation = new RotationData { FromDegrees = 180.0, MidDegrees = 90.0, ToDegrees = 0.0, RotationProperty = "RotationY", PlaneProjection = frontPP, AnimationDuration = Duration };
                        hideBackRotation = new RotationData { FromDegrees = 0.0, MidDegrees = -90.0, ToDegrees = -180.0, RotationProperty = "RotationY", PlaneProjection = backPP, AnimationDuration = Duration };
                        break;
                    case RotationDirection.BottomToTop:
                        backPP.CenterOfRotationX = frontPP.CenterOfRotationX = 0.5;
                        showBackRotation = new RotationData { FromDegrees = 180.0, MidDegrees = 90.0, ToDegrees = 0.0, RotationProperty = "RotationX", PlaneProjection = backPP, AnimationDuration = Duration };
                        hideFrontRotation = new RotationData { FromDegrees = 0.0, MidDegrees = -90.0, ToDegrees = -180.0, RotationProperty = "RotationX", PlaneProjection = frontPP, AnimationDuration = Duration };
                        showFrontRotation = new RotationData { FromDegrees = -180.0, MidDegrees = -90.0, ToDegrees = 0.0, RotationProperty = "RotationX", PlaneProjection = frontPP, AnimationDuration = Duration };
                        hideBackRotation = new RotationData { FromDegrees = 0.0, MidDegrees = 90.0, ToDegrees = 180.0, RotationProperty = "RotationX", PlaneProjection = backPP, AnimationDuration = Duration };
                        break;
                    case RotationDirection.TopToBottom:
                        backPP.CenterOfRotationX = frontPP.CenterOfRotationX = 0.5;
                        showBackRotation = new RotationData { FromDegrees = -180.0, MidDegrees = -90.0, ToDegrees = 0.0, RotationProperty = "RotationX", PlaneProjection = backPP, AnimationDuration = Duration };
                        hideFrontRotation = new RotationData { FromDegrees = 0.0, MidDegrees = 90.0, ToDegrees = 180.0, RotationProperty = "RotationX", PlaneProjection = frontPP, AnimationDuration = Duration };
                        showFrontRotation = new RotationData { FromDegrees = 180.0, MidDegrees = 90.0, ToDegrees = 0.0, RotationProperty = "RotationX", PlaneProjection = frontPP, AnimationDuration = Duration };
                        hideBackRotation = new RotationData { FromDegrees = 0.0, MidDegrees = -90.0, ToDegrees = -180.0, RotationProperty = "RotationX", PlaneProjection = backPP, AnimationDuration = Duration };
                        break;
                }

                FrontElement.RenderTransformOrigin = new Point(.5, .5);
                BackElement.RenderTransformOrigin = new Point(.5, .5);

                FrontElement.Projection = frontPP;
                BackElement.Projection = backPP;

                frontToBackStoryboard.Duration = Duration;
                backToFrontStoryboard.Duration = Duration;

                // Rotation
                frontToBackStoryboard.Children.Add(CreateRotationAnimation(showBackRotation));
                frontToBackStoryboard.Children.Add(CreateRotationAnimation(hideFrontRotation));
                backToFrontStoryboard.Children.Add(CreateRotationAnimation(hideBackRotation));
                backToFrontStoryboard.Children.Add(CreateRotationAnimation(showFrontRotation));

                // Visibility
                frontToBackStoryboard.Children.Add(CreateVisibilityAnimation(showBackRotation.AnimationDuration, FrontElement, false));
                frontToBackStoryboard.Children.Add(CreateVisibilityAnimation(hideFrontRotation.AnimationDuration, BackElement, true));
                backToFrontStoryboard.Children.Add(CreateVisibilityAnimation(hideBackRotation.AnimationDuration, FrontElement, true));
                backToFrontStoryboard.Children.Add(CreateVisibilityAnimation(showFrontRotation.AnimationDuration, BackElement, false));
            }

            if (RotationType == RotationType.Forward)
            {
                if (CurrentStatus != RotationType.Forward)
                {
                    frontToBackStoryboard.Begin();
                    CurrentStatus = RotationType.Forward;
                }
            }
            else
            {
                if (CurrentStatus != RotationType.Backward)
                {
                    backToFrontStoryboard.Begin();
                    CurrentStatus = RotationType.Backward;
                }
            }
        }

        private static ObjectAnimationUsingKeyFrames CreateVisibilityAnimation(Duration duration, DependencyObject element, bool show)
        {
            var animation = new ObjectAnimationUsingKeyFrames();
            animation.BeginTime = new TimeSpan(0);
            animation.KeyFrames.Add(new DiscreteObjectKeyFrame { KeyTime = new TimeSpan(0), Value = (show ? Visibility.Collapsed : Visibility.Visible) });
            animation.KeyFrames.Add(new DiscreteObjectKeyFrame { KeyTime = new TimeSpan(duration.TimeSpan.Ticks / 2), Value = (show ? Visibility.Visible : Visibility.Collapsed) });
            Storyboard.SetTargetProperty(animation, new PropertyPath("Visibility"));
            Storyboard.SetTarget(animation, element);
            return animation;
        }

        private static DoubleAnimationUsingKeyFrames CreateRotationAnimation(RotationData rd)
        {
            var animation = new DoubleAnimationUsingKeyFrames();
            animation.BeginTime = new TimeSpan(0);
            //animation.KeyFrames.Add(new EasingDoubleKeyFrame { KeyTime = new TimeSpan(0), Value = rd.FromDegrees, EasingFunction = new CircleEase() { EasingMode = EasingMode.EaseIn } });
            //animation.KeyFrames.Add(new EasingDoubleKeyFrame { KeyTime = new TimeSpan(rd.AnimationDuration.TimeSpan.Ticks / 2), Value = rd.MidDegrees });
            //animation.KeyFrames.Add(new EasingDoubleKeyFrame { KeyTime = new TimeSpan(rd.AnimationDuration.TimeSpan.Ticks), Value = rd.ToDegrees, EasingFunction = new CircleEase() { EasingMode = EasingMode.EaseOut } });
            animation.KeyFrames.Add(new EasingDoubleKeyFrame { KeyTime = new TimeSpan(0), Value = rd.FromDegrees, EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseIn } });
            animation.KeyFrames.Add(new EasingDoubleKeyFrame { KeyTime = new TimeSpan(rd.AnimationDuration.TimeSpan.Ticks / 2), Value = rd.MidDegrees });
            animation.KeyFrames.Add(new EasingDoubleKeyFrame { KeyTime = new TimeSpan(rd.AnimationDuration.TimeSpan.Ticks), Value = rd.ToDegrees, EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut } });
            Storyboard.SetTargetProperty(animation, new PropertyPath(rd.RotationProperty));
            Storyboard.SetTarget(animation, rd.PlaneProjection);
            return animation;
        }
    }

}
