using System.Text.RegularExpressions;
using System.Globalization;
/*
This class provides the formatting methods to 
ensure that the raw, numerical text is bounded 
on the display.
*/
public class FormattingMethods 
{
   // A regex variable to count the numbers present.
   private static MatchCollection matches;
   // A helper function for editing the raw text.
   public static string numDisplayFormat (double value) {
       // Two variables for storing the fraction and integer parts of a number.
       double fraction, integer;
       // The integer portion is extracted 
       integer = System.Math.Truncate(value);
       // The fraction is extracted
       fraction = value - integer;
       // The doesn't have an integer section
        if (integer == 0 && fraction !=0) {
            // Round the fraction to the nearest 4 decimal places
            double fvalue = System.Math.Round(fraction,4);
            // Is the value extremely small ( < 1/10000 )
            if (System.Math.Abs(value * 10000) < 10)
                // Write it as an exponent 
                return fraction.ToString("e2",CultureInfo.InvariantCulture);
            // Return the edited value.
            return fvalue.ToString();
        }
        // There is an integer part.
        // Round the values to 3 decimal places.
       value = System.Math.Round(value,3);
       // Count the numbers in the result.
       matches = Regex.Matches(value.ToString(),@"\d");
       // If the numbers are more than 8, write it as an exponent.
       if (matches.Count > 8) {
           return value.ToString("e2",CultureInfo.InvariantCulture);
       } return value.ToString();
   }
}
