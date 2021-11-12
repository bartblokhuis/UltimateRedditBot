
import { Config } from '.././interfaces/Config';
import * as File from '../../config.json';
import { Result } from '../data/Result';
import axios, { AxiosRequestConfig, AxiosResponse, AxiosStatic } from 'axios'

export class BaseApiService {

    public config: Config = File as Config
    public axios = axios;
    public baseHeaders: {}

    constructor() {
        this.baseHeaders =  {headers: { 'content-type': 'application/json; charset=utf-8', 'X-Api-Key': this.config.apiKey }};
    }

    protected post<T>(url: string, data): Promise<AxiosResponse<T>>{
        return this.axios.post(this.config.baseUrl + url, data, this.baseHeaders)
        .then((result) => {
            return result;
        })
        .catch((error) => {
            console.log(error);
            return undefined;
        });
    }

    protected get<T>(url: string): Promise<AxiosResponse<T>> {
        return this.axios.get<T>(this.config.baseUrl + url, this.baseHeaders)
        .then((result) => {
            return result;
        })
        .catch((error) => {
            console.log(error);
            return undefined;
        });
    }

    protected put<T>(url: string, data) : Promise<AxiosResponse<T>> {
        return this.axios.put(this.config.baseUrl + url, data, this.baseHeaders)
        .then((result) => {
            return result;
        })
        .catch((error) => {
            console.log(error);
            return undefined;
        });
    }

    protected delete<T>(url: string, data) : Promise<AxiosResponse<T>> { 
        const header = {headers: { 'content-type': 'application/json; charset=utf-8', 'X-Api-Key': this.config.apiKey }, data: data};
        return this.axios.delete(this.config.baseUrl + url, header)
            .then((result) => {
                return result;
            }).catch((error) => {
                console.log(error);
                return undefined;
            })
    }
}