using Zipa;
using Spectre.Console;

var controller = new MainController();

try{
  // if(string.IsNullOrWhiteSpace(args[0])) controller.main();

  if(args.Length>=2){
 
    switch (args[0]){
      case"zip": controller.compress(args); break;
      case"unzip": controller.decompress(); break;
      case"-h":
      case"-help":
      case"--h":
      case"--help":
      case"help": Ui.help(); break;
    }
  }

  else {
    
    if(args.Length==1){
      if( args[0] == "help" || 
        args[0] == "-h" ||
        args[0] == "-help" ||
        args[0] == "--h" ||
        args[0] == "--help")
         Ui.help();
    }

    AnsiConsole.Write(new Rule());
    controller.main();


  }
}catch (Exception e){

  Console.WriteLine(e.Message);
  Console.WriteLine(e.StackTrace);
}

