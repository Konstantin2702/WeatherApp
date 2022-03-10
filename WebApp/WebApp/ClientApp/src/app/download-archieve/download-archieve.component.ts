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
    constructor(private dataService: DataService) {}
  ngOnInit() {
    this.loadFiles();    // загрузка данных при старте компонента 
    
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

