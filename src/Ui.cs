using Spectre.Console;

namespace Zipa;

public class Ui{

  // public void string msgSuccess(string msg="sucesso"){
  //   
  // }

  public static void help(){
    var table = new Table();

    table.Expand = true;
    table.Border = TableBorder.HeavyHead;
    table.AddColumn("[lime]Comando[/]");
    table.AddColumn("[lime]Como Usar[/]");

    table.AddRow(
        "[green]zip[/]",
        "Comprimi arquivos e pastas em um arquivo [blue].Zip[/] ex: [aqua]zip directory1 program.cs directory2[/]");
    
    table.AddRow("[green]unzip[/]","Descompri um arquivo [blue].Zip[/] para uma pasta ex: [aqua]unzip comprimido.zip[/]");
    
    table.AddRow("[green]-h --h -help --help[/]","[blue]Auto descritivo[/]");

    AnsiConsole.Write(table);
  }

  public static void showPath(string path)
    => AnsiConsole.Write(new TextPath(path));

  public static string mainMenu()
    =>AnsiConsole.Prompt<string>(
        new SelectionPrompt<string>()
        .Title("Menu")
        .AddChoices(new []{
          "1. Ver um arquivo compactado",
          "2. Compactar",
          "3. Descompactar",
          "4. Ajuda",
          "5. Sair"
          })
        );
 
  public static string menuSingleChoice(string[] arr, string title="Menu")
    => AnsiConsole.Prompt<string>(
        new SelectionPrompt<string>()
        .Title(title)
        .AddChoices(arr)
        );

  public static void showData(string[] arr){
    
  }

   public static List<string> /* List<FileDTO> */ selectDirs(string[] arr)
    => AnsiConsole.Prompt(
      new MultiSelectionPrompt<string>()
        .Title("Quais arquivos devo compactar")
        .PageSize(30)
        .AddChoices(arr)
      );
  // 
  // for(int i =0;i<=(paths.Count-1);i++){
  //   foreach(string item in selected){
  //
  //     if(paths[i].name == item){
  //       paths[i] = new FileDTO(){
  //           name=paths[i].name,
  //           path=paths[i].path,
  //           dir = paths[i].dir,
  //           selected = true
  //         };  
  //     }
  //   }
  // }
  //   return paths;
  
}
