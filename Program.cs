﻿using System.IO.Compression;
using Spectre.Console;

// é o zip
// ZipFile.CreateFromDirectory("./tozip/","zipado.zip");

void zip(){
  
  for(int i=1;i<=args.Length;i++){
    
  }
}

void unzip(){

}


void directory(){

  var lst = new List<Data>();

  
  var dirs = Directory.GetDirectories(Directory.GetCurrentDirectory());

  for(int i =0;i< dirs.Length;i++){
    var dir = new DirectoryInfo(dirs[i]).Name; 
    lst.Add(new Data( dirs[i],$"[yellow][/] {dir}", true));
  }

  var files = Directory.GetFiles(Directory.GetCurrentDirectory());
    
  for(int i =0;i < files.Length;i++){
    
    var name = new FileInfo(files[i]).Name;
    
    lst.Add(new Data( files[i], $"[blue]󰈙[/]{name}", false));
  }
  
  var selected = AnsiConsole.Prompt(
      new MultiSelectionPrompt<string>()
        .Title("Quais arquivos devo compactar")
        .PageSize(30)
        .AddChoices(lst.Select(x=>x.nome).ToArray())
      );
  Console.Write("\n");
  foreach(var i in selected) AnsiConsole.MarkupLine($"[green]{i}[/]");  
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

public record Data(string path, string nome, bool dir);