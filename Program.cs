using Zipa;

// ZipFile.CreateFromDirectory("./tozip/","zipado.zip");

void zip(){
  // var lst = new List<string>();
  // string zipfileName;
  //
  // while(true){
  //   zipfileName = AnsiConsole.Ask<string>("Qual o nome do arquivo?");
  //   if(!string.IsNullOrEmpty(zipfileName)){
  //     
  //     if(!File.Exists(zipfileName+".zip")) break;
  //     
  //     else  AnsiConsole.MarkupLine("[red]Este arquivo ja existe[/]");
  //   }
  // } 
  //
  // if(args is not null){ 
  //   int lim = args.Length;
  //   // if(lim==1){
  //     // lst = directory();
  //   }
  // }
  // 
  // Directory.CreateDirectory(zipfileName);
  // paths.ForEach(item=>{
  //   Console.WriteLine(item.selected);
  //   if(item.selected){
  //     if(item.dir){
  //   
  //     }else
  //       File.Copy(item.path,zipfileName+@"/"+item.name.Split(" ")[1]); 
  //   }
  // });
  //
  // ZipFile.CreateFromDirectory(zipfileName,zipfileName+".zip");
  // 
  // foreach(string filesPaths in Directory.GetFiles(zipfileName))
  //   File.Delete(filesPaths);
  //
  // Directory.Delete(zipfileName);
  //
  // Console.WriteLine("\n");
  // AnsiConsole.Write(new Rule());
  // AnsiConsole.MarkupLine($"[green]{zipfileName}[/] foi gerado");
}

var controller = new MainController();

try{
  // if(string.IsNullOrWhiteSpace(args[0])) controller.main();
  if(args.Length>=2){
    if(args[0]=="unzip"){
      controller.decompress(args[1]);
    }
  }
  
  else controller.main();
}catch (Exception e){
    
    throw;
}

