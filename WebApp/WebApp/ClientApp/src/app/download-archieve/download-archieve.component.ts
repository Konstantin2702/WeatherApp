import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { DataService } from '../data.service';
import { FileInfo } from '../FileInfo';

@Component({
  selector: 'app-download',
  templateUrl: './download-archieve.component.html',
  providers: [DataService]
})
export class DownloadArchieveComponent {         
  tableMode: boolean = true;        
    files: FileInfo[];
    selectedFile: File[] = null;
    status: string;
    constructor(private dataService: DataService, private http:HttpClient) {}
  ngOnInit() {
    this.loadFiles();    // загрузка данных при старте компонента 
    
}

onFileSelected(event) {
  this.selectedFile = <File[]>event.target.files;
}

onUpload() {
  this.status = "";
  const fd = new FormData();


  for(let i = 0; i < this.selectedFile.length; i++){
  fd.append('files', this.selectedFile[i], this.selectedFile[i].name);
  }
  this.http.post('api/weather/SendFiles', fd)
      .subscribe((data: any) => {
          this.status = data.Text;
      });
    }



// получаем данные через сервис
loadFiles() {
    this.dataService.getFiles()
        .subscribe((data: FileInfo[]) => {
          this.files = data; 
        } 
          );

}
saveinDb(f: FileInfo) {
    this.dataService.saveFile(f)
        .subscribe((data: FileInfo) => 
        {
           f.status = data.status;
           f.isLoaded=true;    
        });
      }
}

