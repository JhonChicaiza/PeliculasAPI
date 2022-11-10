export interface actorDTO{
    id: number;
    nombre: string;
    fechaNacimiento: Date;
    foto: string; //porque de la BD trae una URL
    biografia: string;
}

export interface actorCreacionDTO{
    nombre: string;
    fechaNacimiento: Date;
    foto: File;
    biografia: string;
}