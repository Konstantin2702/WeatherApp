import { Component, Input, OnInit } from '@angular/core';
import { DataService } from '../data.service';
import { FileInfo } from '../FileInfo';
import { WeatherJSON as Weather } from '../WeatherInfo';
@Component({
  selector: 'app-show-archieve',
  templateUrl: './show-archieve.component.html',
  styleUrls: ['./show-archieve.component.css']
})
export class ShowArchieveComponent implements OnInit {

  infWeather: Weather[];
  tableMode: boolean = true;  
  files: FileInfo[];
  jan: string = "привет";
  months = [
  { id: 1, name: 'Январь' },
  { id: 2, name: 'Февраль' },
  { id: 3, name: 'Март' },
  { id: 4, name: 'Апрель' },
  { id: 5, name: 'Май' },
  { id: 6 , name: 'Июнь' },
  { id: 7, name: 'Июль' },
  { id: 8, name: 'Август' },
  { id: 9, name: 'Сентябрь' },
  { id: 10, name: 'Октябрь' },
  { id: 11, name: 'Ноябрь' },
  { id: 12, name: 'Декбрь' }
  ];
  years = [
      2010,2011,2012,2013
    ];
  public selectedMonth = null;
  public selectedYear = null;
  constructor(private dataService: DataService) { }

  ngOnInit() {
  }

  loadWeather() {
    if(this.selectedMonth === null)
      this.selectedMonth = 0;
    if(this.selectedYear === null)
    this.selectedYear = 0;
    this.dataService.getWeather(this.selectedMonth, this.selectedYear)
        .subscribe((data: any) => {
          this.infWeather = data; 
        } 
          );
         
}
}
