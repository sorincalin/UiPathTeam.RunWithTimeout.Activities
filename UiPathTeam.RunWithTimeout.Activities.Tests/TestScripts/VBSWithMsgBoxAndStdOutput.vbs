' Based on code by Kelvin Sung
' File:  digits.vbs
'
' example demonstrates mod (modulus, finding remainder) and integer div (\)
' by breaking a five-digit number into individual digits
 
'Option Explicit                  ' must declare every variables before use

dim inputNumber                  ' input from user
dim origNumber                   ' save the inputNumber
dim onesDigit, tenDigit, hunDigit, thouDigit, tenThouDigit

Set args = Wscript.Arguments

For Each arg In args
  Wscript.Echo arg
Next

MsgBox "Ready to start?"
 
inputNumber = 0

do
   ' check for validity
   
   inputNumber = inputNumber + 1
   if inputNumber = 1000 then
       exit do
   end if
   WScript.Echo inputNumber
   
loop

MsgBox "All done!"


