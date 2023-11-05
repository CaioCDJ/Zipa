using System.IO.Compression;
using Spectre.Console;

var paths = new List<Data>();

// é o zip
// ZipFile.CreateFromDirectory("./tozip/","zipado.zip");

void zip(){
  var lst = new List<string>();
  string zipfileName;

  while(true){
    zipfileName = AnsiConsole.Ask<string>("Qual o nome do arquivo?");
    if(!string.IsNullOrEmpty(zipfileName)){
      
      if(!File.Exists(zipfileName+".zip")) break;
      
      else  AnsiConsole.MarkupLine("[red]Este arquivo ja existe[/]");
    }
  } 

  if(args is not null){ 
    int lim = args.Length;
    if(lim==1){
      lst = directory();
    }
  }
  
  Directory.CreateDirectory(zipfileName);
  paths.ForEach(item=>{
    Console.WriteLine(item.selected);
    if(item.selected){
      if(item.dir){
    
      }else
        File.Copy(item.path,zipfileName+@"/"+item.name.Split(" ")[1]); 
    }
  });

  ZipFile.CreateFromDirectory(zipfileName,zipfileName+".zip");
  
  foreach(string filesPaths in Directory.GetFiles(zipfileName))
    File.Delete(filesPaths);

  Directory.Delete(zipfileName);

  Console.WriteLine("\n");
  AnsiConsole.Write(new Rule());
  AnsiConsole.MarkupLine($"[green]{zipfileName}[/] foi gerado");
}

void unzip(){

  var file = new FileInfo(args[1]);

  string name = file.Name.Remove((file.Name.Length-4) , 4);

  int i = 1;
  
  while(true){
    
    if(!Directory.Exists(name)) break;
    
    else if(!Directory.Exists(name+i)){
      name = name +i;
      break;
    }
    i++;
  }

  ZipFile.ExtractToDirectory(
      file.FullName,
      file.Directory.FullName +@"/"+name
      );

  AnsiConsole.MarkupLine($"\n[green]{name} foi descompactado![/]");

}

List<string> directory(){
  
  var dirs = Directory.GetDirectories(Directory.GetCurrentDirectory());

  for(int i =0;i< dirs.Length;i++){
    var dir = new DirectoryInfo(dirs[i]).Name; 
    paths.Add(new Data( ){
        path= dirs[i],
        name = $"[yellow][/] {dir}",
        dir = true
        });
  }

  var files = Directory.GetFiles(Directory.GetCurrentDirectory());
    
  for(int i =0;i < files.Length;i++){
    
    var name = new FileInfo(files[i]).Name;
    paths.Add(new Data(){
        path = files[i],
        name =  $"[blue]󰈙[/] {name}",
        dir = false
        });  
    // paths.Add(new Data( files[i], $"[blue]󰈙[/] {name}", false));
  }
  
  var selected = AnsiConsole.Prompt(
      new MultiSelectionPrompt<string>()
        .Title("Quais arquivos devo compactar")
        .PageSize(30)
        .AddChoices(paths.Select(x=>x.name).ToArray())
      );
  // Console.Write("\n");
  // foreach(var i in selected) AnsiConsole.MarkupLine($"[green]{i}[/]");  

  
  for(int i =0;i<=(paths.Count-1);i++){
    foreach(string item in selected){

      if(paths[i].name == item){
        paths[i] = new Data(){
            name=paths[i].name,
            path=paths[i].path,
            dir = paths[i].dir,
            selected = true
          };  
      }
    }
  }
  return selected;
}

if(args.Length>0){
// if(args[0] is not null || !String.IsNullOrEmpty(args[0])){

  switch(args[0].ToLower()){
    case "zip":
      zip();    
      break;
    case "unzip":
      unzip();
      break;
  }
}else
  directory();

public class Data{
  public string path;
  public string name;
  public bool dir {get;set;}=false;
  public bool selected {get;set;}=false;
}
