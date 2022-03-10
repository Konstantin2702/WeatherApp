import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FileInfo } from './FileInfo';
import { DataService } from './data.service';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  providers: [DataService]
})
export class AppComponent implements OnInit {
    tableMode: boolean = true;          // табличный режим
    str: String = "";
    files: FileInfo[];
  constructor(private dataService: DataService) {}
  ngOnInit() {
    this.loadProducts();    // загрузка данных при старте компонента  
}
// получаем данные через сервис
loadProducts() {
    this.dataService.getFiles()
        .subscribe((data: FileInfo[]) => this.files = data);
}
}
