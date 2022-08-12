namespace Availity.Lisp.Validator;

public static class Program
{
    public static void Main(string[] args)
    {
        string validationMessage;

        // Example flattened from code from Stanford University
        var lisp = @"(defun filter(list predicate)(if (null list) '()(let ((the-rest (filter (cdr list) predicate)))(if (funcall predicate (car list))(cons (car list) the-rest)the-rest))))";
        // Missing ending parenthesis
        var badLisp = @"(defun filter(list predicate)(if (null list) '()(let ((the-rest (filter (cdr list) predicate)))(if (funcall predicate (car list))(cons (car list) the-rest)the-rest)))";


        validationMessage = CheckLisp(string.Empty);
        Console.WriteLine(validationMessage);

        validationMessage = CheckLisp(lisp);
        Console.WriteLine(validationMessage);

        validationMessage = CheckLisp(badLisp);
        Console.WriteLine(validationMessage);

        Console.ReadLine();
    }

    private static string CheckLisp(string lispString)
    {

        var error = false;
        string validationMessage;

        if (string.IsNullOrWhiteSpace(lispString))
        {
            return $"Null String Unable to Validate";
        }


        var brackets = new Stack<char>();

        foreach (var c in lispString)
        {
            if (c == '[' || c == '{' || c == '(' || c == '<')
            {
                brackets.Push(c);
            }                
            else if (c == ']' || c == '}' || c == ')' || c == '>')
            {
                // Too many closing brackets, e.g. (123))
                if (brackets.Count <= 0)
                {
                    error = true;
                    break;
                }
                else
                {
                    var open = brackets.Pop();
                    // Inconsistent brackets, e.g. (123]
                    if (c == '}' && open != '{' ||
                        c == ')' && open != '(' ||
                        c == ']' && open != '[' ||
                        c == '>' && open != '<')
                    {
                        error = true;
                        break;
                    }
                }
            }
        }

        // Too many opening brackets, e.g. ((123) 
        if (brackets.Count > 0)
        {
            error = true;
        }

        if (error)
        {
            validationMessage = $"{lispString} is Not a valid Lisp Expression";
        }
        else
        {
            validationMessage = $"{lispString} is a valid Lisp Expression";
        }

        return validationMessage;
    }



}
