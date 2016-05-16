using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Util;
using System.Xml.Linq;
using Java.Lang;

namespace ColPattAndroidAssignment
{
    public class CustomShapeView : View
    {
        /** Paint to draw shapes */
        private Paint mPaint = null;        

        /** All available shapes */
        private HashSet<Shape> mShapes = new HashSet<Shape>();
        private SparseArray<Shape> mShapePointer = new SparseArray<Shape>(Util.SHAPES_LIMIT);
        Context mContext;

        /**
         * Default constructor
         *
         * @param ct {@link android.content.Context}
         */
        public CustomShapeView(Context context) : base(context)
        {
            init(context);
        }

        public CustomShapeView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            init(context);
        }

        public CustomShapeView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
            init(context);
        }

        private void init(Context ct)
        {
            //initializing paint for doing the drawing operations
            mContext = ct;
            mPaint = new Paint() { Color = Color.Red };
            mPaint.SetStyle(Paint.Style.Fill);
        }

        protected override void OnDraw(Canvas canvas)
        {
            foreach (Shape myShape in mShapes)
            {
                Log.Warn(Util.TAG, "Drawing Circle :" + myShape.centerX + " : " + myShape.centerY + " : " + myShape.radius + " : isCircle : " + myShape.isCircle+" : "+ myShape.getIsWebServiceCallSuccess());
                Bitmap sqBmp = null;
                if (myShape.getIsWebServiceCallSuccess()) {
                    if (myShape.isCircle == false)
                    {
                        sqBmp = myShape.getSquareBitmap();
                        if (sqBmp != null)
                            canvas.DrawBitmap(sqBmp, myShape.centerX, myShape.centerY, mPaint);
                    }
                    else
                    {
                        int[] mColors = myShape.getColorsValueAsArray(); 
                        if (mColors != null && mColors.Length > 2)
                            mPaint.SetARGB(255, mColors[0], mColors[1], mColors[2]);

                        mPaint.SetStyle(Paint.Style.Fill);
                        canvas.DrawCircle(myShape.centerX, myShape.centerY, myShape.radius, mPaint);
                    }
                }
                else
                {
                    int[] mColors = myShape.getColorsValueAsArray(); 
                    if (mColors != null && mColors.Length > 2)
                        mPaint.SetARGB(255, mColors[0], mColors[1], mColors[2]);

                    if (myShape.isCircle == false)
                    {
                        Rect rect = new Rect(myShape.centerX, myShape.centerY, myShape.centerX + Util.RECT_SIZE, myShape.centerY + Util.RECT_SIZE);
                        mPaint.SetStyle(Paint.Style.Fill);
                        canvas.DrawRect(rect, mPaint);
                    }                    
                    else
                    {
                        mPaint.SetStyle(Paint.Style.Fill);
                        canvas.DrawCircle(myShape.centerX, myShape.centerY, myShape.radius, mPaint);
                    }
                }
            }
        }

        public override bool OnTouchEvent(MotionEvent me)
        {
            bool handled = false;
            Shape touchedShape = null;
            int xTouch;
            int yTouch;
            int pointerId;
            int actionIndex = me.ActionIndex;
            switch (me.ActionMasked)
            {
                case MotionEventActions.Up:
                    Log.Warn(Util.TAG, "Pointer Up");
                    clearShapePointer();
                    Invalidate();
                    handled = true;
                    break;
                case MotionEventActions.Down:
                    Log.Warn(Util.TAG, "Pointer down");
                    clearShapePointer();

                    xTouch = (int)me.GetX(actionIndex);
                    yTouch = (int)me.GetY(actionIndex);

                    //Log.Warn(TAG,"Touch Position : " + xTouch + " : " + yTouch);
                    // check if we've touched inside some shape
                    touchedShape = obtainTouchedShape(xTouch, yTouch);
                    touchedShape.centerX = xTouch;
                    touchedShape.centerY = yTouch;
                    mShapePointer.Put(me.GetPointerId(actionIndex), touchedShape);

                    Invalidate();
                    handled = true;
                    break;
                case MotionEventActions.Move:
                    int pointerCount = 2;
                    pointerCount = me.PointerCount;
                    Log.Warn(Util.TAG, "Move");

                    for (actionIndex = 0; actionIndex < pointerCount; actionIndex++)
                    {
                        // Some pointer has moved, search it by pointer id
                        pointerId = me.GetPointerId(actionIndex);

                        xTouch = (int)me.GetX(actionIndex);
                        yTouch = (int)me.GetY(actionIndex);

                        touchedShape = mShapePointer.Get(pointerId);

                        if (null != touchedShape)
                        {
                            touchedShape.centerX = xTouch;
                            touchedShape.centerY = yTouch;
                        }
                    }

                    Invalidate();
                    handled = true;
                    break;
                case MotionEventActions.Cancel:
                    handled = true;
                    break;
                default:
                    break;
            }
            return handled;
        }

        /**
         * Clears all Shapes - pointer id relations
         */
        private void clearShapePointer()
        {
            Log.Warn(Util.TAG, "Clear Shape Pointer");
            mShapePointer.Clear();
        }

        public void clearAllShapes()
        {
            mShapes.Clear();
            clearShapePointer();
        }

        /**
         * Search and creates new shape (if needed) based on touch area
         *
         * @param xTouch int x of touch
         * @param yTouch int y of touch
         *
         * @return obtained {@link Shape}
         */
        public Shape obtainTouchedShape(int xTouch, int yTouch)
        {
            Shape touchedShape = getTouchedShape(xTouch, yTouch);

            if (null == touchedShape)
            {
                bool isCircleFlag = false;
                int randNo = Util.getRandomIntValue(0, Util.SHAPES_LIMIT);

                if (randNo % 2 == 0)
                    isCircleFlag = true;

                touchedShape = new Shape(xTouch, yTouch, Util.CIRCLE_RADIUS, isCircleFlag);

                if (mShapes.Count == Util.SHAPES_LIMIT)
                {
                    Log.Warn(Util.TAG, "Clear all Shapes, size is " + mShapes.Count);
                    // remove first circle
                    mShapes.Clear();
                }

                if (isCircleFlag)
                    touchedShape = callWebServiceAsync(0, touchedShape);
                else
                    touchedShape =  callWebServiceAsync(1, touchedShape);

                Log.Warn(Util.TAG, "Added new Shape object after webservice: " + touchedShape);
                mShapes.Add(touchedShape);
            }

            return touchedShape;
        }


        /**
         * Determines touched circle
         *
         * @param xTouch int x touch coordinate
         * @param yTouch int y touch coordinate
         *
         * @return {@link Shape} touched shape or null if no shape has been touched
         */
        private Shape getTouchedShape(int xTouch, int yTouch)
        {
            Shape touchedShape = null;

            foreach (Shape circle in mShapes)
            {
                Log.Warn(Util.TAG, "Touched Shape is Cirlce : " + circle.isCircle+" : "+ circle.centerX+" : "+ xTouch+" : "+ circle.radius);
                if (circle.isCircle)
                {
                    if ((circle.centerX - xTouch) * (circle.centerX - xTouch) + (circle.centerY - yTouch) * (circle.centerY - yTouch) <= circle.radius * circle.radius)
                    {
                        touchedShape = circle;
                        break;
                    }
                }
                else
                {
                    var x = circle.centerX + circle.radius;
                    var y = circle.centerY + circle.radius;

                    if ((x - xTouch) * (x - xTouch) + (y - yTouch) * (y - yTouch) <= circle.radius * circle.radius)
                    {
                        touchedShape = circle;
                        break;
                    }
                }
            }

            return touchedShape;
        }

        public Shape callWebServiceAsync(int pattern, Shape shapeObj)
        {
            if (pattern == 0)
                shapeObj = getColorValueFromWebservice(shapeObj);
            else
                shapeObj = getPatternValueFromWebservice(shapeObj);

            return shapeObj;
        }

        public Shape getColorValueFromWebservice(Shape cirlceObj) // assume we return an int from this long running operation 
        {
            XDocument xDOC = Network.getXDocumentFromUrl(Util.url_color);
            if(xDOC != null)
            {
                // Console.WriteLine("XMl Value :"+X);
                var red = xDOC.Element("colors").Element("color").Element("rgb").Element("red").Value.ToString();
                var green = xDOC.Element("colors").Element("color").Element("rgb").Element("green").Value.ToString();
                var blue = xDOC.Element("colors").Element("color").Element("rgb").Element("blue").Value.ToString();
                Console.WriteLine("Color Value : " + red + " : " + green + " : " + blue);

                if (red != null && green != null && blue != null)
                {
                    cirlceObj.setWebServiceCallSuccess(true);
                    cirlceObj.setColors(Integer.ParseInt(red), Integer.ParseInt(green), Integer.ParseInt(blue));
                }
                else
                {
                    cirlceObj.setWebServiceCallSuccess(false);
                    cirlceObj = setRandomColorsForShapes(cirlceObj);
                }
            }
            else
            {
                cirlceObj.setWebServiceCallSuccess(false);
                cirlceObj = setRandomColorsForShapes(cirlceObj);
            }
               

            return cirlceObj;
        }

        public Shape getPatternValueFromWebservice(Shape cirlceObj)
        {
            XDocument xDoc = Network.getXDocumentFromUrl(Util.url_pattern);
            // Console.WriteLine("XMl Value :"+X);
            var imgUrl = "";

            if(xDoc != null)
                imgUrl = xDoc.Element("patterns").Element("pattern").Element("imageUrl").Value.ToString();
            Console.WriteLine("Image Url : " + imgUrl);

            if (imgUrl != null && imgUrl != "" && imgUrl.Length > 10)
            {
                cirlceObj.setWebServiceCallSuccess(true);
                cirlceObj.setImageUrl(imgUrl);
                cirlceObj.setSquareBitmap(Util.getBitmapFromUrl(imgUrl));
            }
            else
            {
                cirlceObj.setWebServiceCallSuccess(false);
                cirlceObj.setSquareBitmap(null);
                cirlceObj = setRandomColorsForShapes(cirlceObj);
            }

            return cirlceObj;
        }//fn getPatternValueFromWebservice

        private Shape setRandomColorsForShapes(Shape cirlceObj)
        {
            int[] colors = Util.getRandomColorsValueAsArray();
            cirlceObj.setColors(colors[0], colors[1], colors[2]);
            return cirlceObj;
        }

    }//End of class CustomShapeView
}