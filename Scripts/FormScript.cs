using System.Collections.Generic;
/*
 This class is used  for Message-passing between 
 Form UI elemnets: the file loader and the UIform
*/
public class FormScript
{
    // A variable for passing the key-value pairs.
    public static Dictionary<string, double []> data;
    // A variable to synchronize behaviours.
    public static int messageState = 0;
}
