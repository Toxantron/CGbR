# CGbR Generator
This is the core generator project. Generating the code from class files is a 4 stage process.

1. It reads a file or directory and runs the [parser](https://github.com/Toxantron/CGbR/tree/master/CGbR/Parser) on every file it finds. 
2. For [project mode](https://github.com/Toxantron/CGbR/blob/master/CGbR/Modes/ProjectMode.cs) class references [are linked](https://github.com/Toxantron/CGbR/blob/master/CGbR/Modes/ProjectMode.cs#L72)
3. The [local generators](https://github.com/Toxantron/CGbR/blob/master/CGbR/ILocalGenerator.cs) are executed on each individual file.
4. The [global generator](https://github.com/Toxantron/CGbR/blob/master/CGbR/IGlobalGenerator.cs) is executed on all classes.
