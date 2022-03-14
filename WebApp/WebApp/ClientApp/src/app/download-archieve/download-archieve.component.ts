import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { DataService } from '../data.service';


@Component({
  selector: 'app-download',
  templateUrl: './download-archieve.component.html',
  providers: [DataService]
})
export class DownloadArchieveComponent {         
  tableMode: boolean = true;        
    selectedFile: File[] = null;
    status: string;
    isLoading:boolean = false;
    constructor(private dataService: DataService, private http:HttpClient) {}
  ngOnInit() { 
}

onFileSelected(event) {
  this.selectedFile = <File[]>event.target.files;
}

onUpload() {
  this.status = "";
  this.isLoading = true;
  const fd = new FormData();


  for(let i = 0; i < this.selectedFile.length; i++){
  fd.append('files', this.selectedFile[i], this.selectedFile[i].name);
  }
  this.http.post('api/weather/SendFiles', fd)
      .subscribe((data: any) => {
          this.status = data.Text;
          this.isLoading = false;
      });
    }
  }

