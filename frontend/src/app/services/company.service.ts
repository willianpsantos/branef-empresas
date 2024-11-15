import { ApiResponse } from '../models/api-response';
import { HttpClient, HttpParams } from '@angular/common/http';
import { API_URL, API_VERSION } from '../injection.tokens';
import { Inject, Injectable } from '@angular/core';
import { CompanyModel } from "../models/company";
import { Observable } from 'rxjs';
import { ListOptions } from '../models/list.options';
import { PageResponse } from '../models/page-response';
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CompanyService
{
  baseUrl: string = 'companies';

  constructor(
    private http: HttpClient,
    @Inject(API_URL) private apiUrl: string,
    @Inject(API_VERSION) private apiVersion: string
  ) {
    this.baseUrl = `${this.apiUrl}/${this.apiVersion}/${this.baseUrl}`;
  }

  listAll(options: ListOptions): Observable<PageResponse<CompanyModel>> {
    const body = options.toBody();

    const prm = new HttpParams({
      fromObject: body
    });

    return this
      .http
      .get<PageResponse<CompanyModel>>(`${this.baseUrl}/${options.page}/${options.take}/paginated/`, { params: prm });
  }

  save(data: CompanyModel) : Observable<ApiResponse<CompanyModel>> {
    if(data.Id) {
      return this
        .http
        .put(this.baseUrl + '/' + data.Id, data)
        .pipe(
          map((response:any) => response as ApiResponse<CompanyModel>)
        )
    }
    else {
        return this
          .http
          .post(this.baseUrl, data)
          .pipe(
            map((response:any) => response as ApiResponse<CompanyModel>)
          )
    };
  }

  delete(id: number) : Observable<ApiResponse<CompanyModel>> {
    return this
      .http
      .delete(this.baseUrl + '/' + id)
      .pipe(
        map((response:any) => response as ApiResponse<CompanyModel>)
      )
  }
}
