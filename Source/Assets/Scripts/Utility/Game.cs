/* Class: Game
 * Author: Gavin Lu
 * 
 * This helper class contains useful functions that could be reused in many areas.
 */

public class Game 
{

    /* Bounds
     * Input:
     *  float x:    Value to be bounded
     *  float min:  Value that x cannot be lower than
     *  float max:  Value that x cannot be higher than
     *  
     * Output: value x that is not lower than min or higher than max
     */
    public static float Bound(float x, float min, float max)
    {
        if (x < min)
            return min;
        else if (x > max)
            return max;

        return x;
    }
}
