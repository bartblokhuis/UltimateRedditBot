export abstract class URLHelper {

    private static imageExtensions: string[] = ['.jpg', 'jpeg', '.png', '.gif', '.gifv'];
    private static videoExtensions : string[] = ['.mp4'];
    private static knownVideoDomains : string[] = ['https://gfycat', 'https://redgifs', 'https://www.redgifs', 'https://imgur', 'https://i.imgur'];

    static isVideoOrImage(url: string) : boolean {
        return this.hasImageExtension(url) || this.isVideo(url, true);
    }

    static isVideo(url: string, skipImageCheck: boolean = false) : boolean {
        
        //Ensure that the url is not an image.
        if(!skipImageCheck && this.hasImageExtension(url)) return false;

        //Check if the url is one of the known video domains or has a video extension at the end of the url.
        return this.hasVideoDomain(url) || this.hasVideoExtension(url);
    }

    private static hasVideoDomain(url: string) : boolean {
        let hasDomain = false;
        this.knownVideoDomains.forEach((videoDomain) => {
            if(url.startsWith(videoDomain)) hasDomain = true;
        });

        return hasDomain;
    }

    private static hasImageExtension(url: string) : boolean {
        return this.hasExtension(url, this.imageExtensions);
    }

    private static hasVideoExtension(url: string) : boolean {
        return this.hasExtension(url, this.videoExtensions);
    }

    private static hasExtension(url: string, allowedExtensions: string[]) : boolean{  
        let hasExtension = false;
        allowedExtensions.forEach((fileExtension) => {
            if(url.endsWith(fileExtension)) hasExtension = true;
        });

        return hasExtension;
    }

}