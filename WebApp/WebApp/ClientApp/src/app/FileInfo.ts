export class FileInfo {
    constructor(
        public fileNames: String,
        public status: String,
        public isLoaded: boolean) {
            status = "";
            isLoaded = false;
        }
       
}