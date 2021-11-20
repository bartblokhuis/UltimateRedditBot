
import consola from 'consola';
import { Config } from '.././interfaces/Config';
import * as File from '../../config.json';
import axios, { AxiosResponse } from 'axios'

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
            consola.error('Request failed with status code ' + error.response.status, error.response.config.url);
            return undefined;
        });
    }

    protected get<T>(url: string): Promise<AxiosResponse<T>> {
        return this.axios.get<T>(this.config.baseUrl + url, this.baseHeaders)
        .then((result) => {
            return result;
        })
        .catch((error) => {
            consola.error('Request failed with status code ' + error.response.status, error.response.config.url);
            return undefined;
        });
    }

    protected put<T>(url: string, data) : Promise<AxiosResponse<T>> {
        return this.axios.put(this.config.baseUrl + url, data, this.baseHeaders)
        .then((result) => {
            return result;
        })
        .catch((error) => {
            consola.error('Request failed with status code ' + error.response.status, error.response.config.url);
            return undefined;
        });
    }

    protected delete<T>(url: string, data) : Promise<AxiosResponse<T>> { 
        const header = {headers: { 'content-type': 'application/json; charset=utf-8', 'X-Api-Key': this.config.apiKey }, data: data};
        return this.axios.delete(this.config.baseUrl + url, header)
            .then((result) => {
                return result;
            }).catch((error) => {
                consola.error('Request failed with status code ' + error.response.status, error.response.config.url);
                return undefined;
            })
    }
}