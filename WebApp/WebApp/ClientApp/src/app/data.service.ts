import { Injectable } from '@angular/core';
import { HttpClient, HttpEvent, HttpRequest} from '@angular/common/http';
import { FileInfo } from './FileInfo';

 
@Injectable({ providedIn: 'root' })
export class DataService {
 
    private url = "/api/weather";
 
    constructor(private http: HttpClient) {
    }
 
    getFiles() {
        return this.http.get(this.url);
    }
     
    saveFile(file: FileInfo) {
        return this.http.post(this.url, file);
    }

    getWeather(month: number, year: number){
        let tempUrl = this.url + "/GetWeather";
        return this.http.get(tempUrl + "?" + "month=" + month + "&year=" + year);
    }
    

}