using System.IO.Compression;
using Spectre.Console;

namespace Zipa;

class MainController{

  public List<DTO>dto;
  public DirectoryInfo mainDir;

  public MainController(){
      dto = new List<DTO>();
  }

  public void main(){
    
    char choice = Ui.mainMenu().ToCharArray()[0];
    
    switch(choice){
      case '1': show(); break;
      case '2': compress(); break;
      case '3': decompress(); break;
      case '5': AnsiConsole.MarkupLine("[red]There is no hope[/]"); break; 
      case '4': 
      default: 
          Environment.Exit(0); break;
    }
  }

  public void show(string arq =null){

    string filepath = arq ?? getFilePath();
   
    var itemFile = ZipFile.Open(filepath,ZipArchiveMode.Read).Entries;
    foreach(var item in itemFile){
      Console.WriteLine(item.GetType());
    }
    Console.WriteLine(itemFile);
    
  }
  
  public void compress(string[] args=null){
 
    string filename = AnsiConsole.Ask<string>("File name:") ?? "compact";

    listItems();
    
    var selected = Ui.selectDirs(
        dto.Select(x=>x.name).ToArray()
    );

    if(selected.Count<=0) throw new Exception();

    for(int i =0;i<=(dto.Count-1);i++){
      foreach(string item in selected){
        if(dto[i].name == item){
          dto[i] = new DTO(){
              name=dto[i].name,
              path=dto[i].path,
              dir = dto[i].dir,
              selected = true
            };  
        }
      }
    }


    using(var fs = new FileStream(filename+".zip",FileMode.Create))
    using(var zip  = new ZipArchive(fs, ZipArchiveMode.Create)){
      foreach(var file in dto.Where(x=>x.selected ==true && x.dir ==false))
        zip.CreateEntryFromFile(file.path,Path.GetFileName(file.path));

      foreach(var file in dto.Where(x=>x.selected ==true && x.dir == true)){
        var dir = new DirectoryInfo(file.path);
        var father = dir.Parent;

        foreach(var path in Directory.GetFiles(dir.FullName,"*",SearchOption.AllDirectories)){
            zip.CreateEntryFromFile(
            path,
            path.Replace(father.FullName,"")
          );
        }
      }
    }

    AnsiConsole.Write(new Rule());
    AnsiConsole.MarkupLine($"[green]{filename}.zip[/] foi gerado com sucesso");
  }
  
  public void decompress(string  arq = null){

    string filepath = arq ?? getFilePath();

    var file = new FileInfo(filepath);

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

  public void listItems(){
    var dirs = Directory.GetDirectories(Directory.GetCurrentDirectory());

    for(int i =0;i< dirs.Length;i++){
      var dir = new DirectoryInfo(dirs[i]).Name; 
      dto.Add(new DTO( ){
        path= dirs[i],
        name = $"[yellow][/] {dir}",
        dir = true
      });
    }

    var files = Directory.GetFiles(Directory.GetCurrentDirectory());
    
    for(int i =0;i < files.Length;i++){
      var name = new FileInfo(files[i]).Name;
      dto.Add(new DTO(){
        path = files[i],
        name =  $"[blue]󰈙[/] {name}",
        dir = false
      });  
    }
  }

  private string getFilePath(){    

    listItems();

    string choice = Ui.menuSingleChoice(
        dto.Where(x=>x.dir==false).Select(x=>x.name).ToArray(),
        title:"Selecione o Arquivo a ser descompactado"
    );     
 
    if(string.IsNullOrEmpty(choice))
      throw new Exception();
    
    return dto.Where(x=>x.name==choice).Select(x=>x.path).First();
  }
}
