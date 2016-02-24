# ComplexTypeComparer
C# Complex Type Comparison Class Using Reflection

```C#
var cat1 = new Cat() { Head = "head", Tail = "tail" };
var cat2 = new Cat() { Head = "head", Tail = "tail" };
var result = ValueComparer.AreEqual(cat1, cat2); // result == true
```
