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
      case '4': Ui.help(); break;
      case '5': 
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

    var total = 0;

    for(int i =0;i<=(dto.Count-1);i++){
      foreach(string item in selected){
        if(dto[i].name == item){
          dto[i] = new DTO(){
            name=dto[i].name,
            path=dto[i].path,
            dir = dto[i].dir,
            selected = true
          };  
          
          total += (dto[i].dir)
            ? Directory.GetFiles(dto[i].path,"*",SearchOption.AllDirectories).Count()
            : 1; 
          
        }
      }
    }

    AnsiConsole.Progress()
     .Columns(new ProgressColumn[]{
        new ProgressBarColumn(),
        new TaskDescriptionColumn(),
      })
      .Start(ctx => {
    
        var task1 = ctx.AddTask("[blue]Compactando[/]",maxValue:total);
             
        using(var fs = new FileStream(filename+".zip",FileMode.Create))
        using(var zip  = new ZipArchive(fs, ZipArchiveMode.Create)){
          foreach(var file in dto.Where(x=>x.selected ==true && x.dir ==false)){
            zip.CreateEntryFromFile(file.path,Path.GetFileName(file.path));
            task1.Description = $"󰈙 {file.name} compactado";
            task1.Increment(1);
          }
          foreach(var file in dto.Where(x=>x.selected ==true && x.dir == true)){
            var dir = new DirectoryInfo(file.path);
            var father = dir.Parent;

            foreach(var path in Directory.GetFiles(dir.FullName,"*",SearchOption.AllDirectories)){
              zip.CreateEntryFromFile(
              path,
              path.Replace(father.FullName,"")
            );
            task1.Description = $"󰈙 {new FileInfo(path).Name} compactado";
            task1.Increment(1);   
          }
        }
      }
        task1.StopTask();
      });
    AnsiConsole.Write(new Rule());
    AnsiConsole.MarkupLine($"[green]{filename}.zip[/] foi gerado com sucesso!");
  }
  
  public void decompress(string  arq = null){

    string filepath = arq ?? getFilePath();

    var file = new FileInfo(filepath);

    AnsiConsole.Status()
      .AutoRefresh(false)
      .Spinner(Spinner.Known.Star)
      .SpinnerStyle(Style.Parse("green bold"))
      .Start($"Descompactado {file.Name}...", ctx => {
    
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

      Directory.CreateDirectory(name);
      var mainDir = new DirectoryInfo(name);

      using ZipArchive archive = ZipFile.OpenRead(file.FullName);

      foreach(var entry in archive.Entries){
        string destination = mainDir.FullName+"//"+ entry.FullName;      
      
        Directory.CreateDirectory(Path.GetDirectoryName(destination)); 
   
      if(!entry.FullName.EndsWith("/") || entry.FullName.EndsWith("//"))
          entry.ExtractToFile(destination,true);
      }
       AnsiConsole.MarkupLine($"\n[green]{name} foi descompactado![/]");

    });

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
