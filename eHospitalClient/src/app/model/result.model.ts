export class ResultModel<T>{
    data?:T ; 
    errorMessage? : string[] ;
    isSuccessfull : boolean = true;
    statusCode: number = 200;
}