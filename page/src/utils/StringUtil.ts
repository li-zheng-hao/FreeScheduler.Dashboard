export function getPathExtension (path:string) {
  const basename = path.split(/[\\/]/).pop() || ''; // extract file name from full path ...
  // (supports `\\` and `/` separators)
  const pos = basename.lastIndexOf('.'); // get last position of `.`

  if (basename === '' || pos < 1) {
    return '';
  } // if file name is empty or ...
  return basename.slice(pos + 1).toLowerCase(); // extract extension ignoring `.`
}
export function convertUTCDateToLocalDate (date:any) {
  const newDate = new Date(date.getTime() + date.getTimezoneOffset() * 60 * 1000);

  const offset = date.getTimezoneOffset() / 60;
  const hours = date.getHours();

  newDate.setHours(hours - offset);

  return newDate;
}
