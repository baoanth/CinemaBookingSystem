export class Province{
    provinceId? : number;
    provinceName : string;
    region : string;

    constructor(provinceId : number, provinceName : string, region: string){
        this.provinceId = provinceId;
        this.provinceName = provinceName;
        this.region = region;
    }
}