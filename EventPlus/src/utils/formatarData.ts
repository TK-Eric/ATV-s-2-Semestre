export function formatarDataLonga(data: string | Date): string {
    if (!data) return "";
    const dataObj = typeof data === "string" ? new Date(data) : data;
    if (isNaN(dataObj.getTime())) return String(data);

    return dataObj.toLocaleDateString("pt-BR", {
        day: "2-digit",
        month: "long",
        year: "numeric",
    });
}

export function formatarDataCurta(data: string | Date): string {
    if (!data) return "";
    const dataObj = typeof data === "string" ? new Date(data) : data;
    if (isNaN(dataObj.getTime())) return String(data);

    return dataObj.toLocaleDateString("pt-BR");
}
