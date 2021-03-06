import { Injectable } from '@angular/core';
import { HttpClient, HttpEvent, HttpRequest} from '@angular/common/http';

 
@Injectable({ providedIn: 'root' })
export class DataService {
 
    private url = "/api/weather";
 
    constructor(private http: HttpClient) {
    }

    getWeather(month: number, year: number, pageNumber: number, countOFElementsOnPage: number){
        let tempUrl = this.url + "/GetWeather";
        return this.http.get(tempUrl + "?" + "month=" + month + "&year=" + year + "&pageNumber=" +
         pageNumber + "&countOFElementsOnPage=" + countOFElementsOnPage);
    }

    getCountOfElementsToShow(month: number, year: number)
    {
        let tempUrl = this.url + "/GetCountWeather";
        return this.http.get(tempUrl + "?" + "month=" + month + "&year=" + year);
    }
}