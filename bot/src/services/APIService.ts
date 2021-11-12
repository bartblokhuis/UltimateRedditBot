import axios, { AxiosRequestConfig, AxiosResponse, AxiosStatic } from 'axios'
import { Service } from 'typedi';
process.env.NODE_TLS_REJECT_UNAUTHORIZED = '0';

@Service()
export class APIService{
    
    private instance: any;

    public constructor(){
    }

    public async get<Type>(url: string, config?: AxiosRequestConfig): Promise<Type>{
        return axios.get<Type>(url, config).then((result) => {
            return result.data;
        }).catch((reason) => {
            return null;
        });
    }

    public async post<Type>(url: string, content: any, config?: AxiosRequestConfig): Promise<Type>{
        
        return axios.post<Type>(url, JSON.stringify(content), {headers: { 'content-type': 'application/json; charset=utf-8' }}).then((result =>{
            return result.data;

        })).catch((reason) =>{
            return null;

        })
    }
    
}