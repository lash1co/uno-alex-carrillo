const apiUrl =
  import.meta.env.VITE_API_URL || "";

export const apiOrigin =
  /^https?:\/\//i.test(apiUrl)
    ? new URL(apiUrl).origin
    : "";

export const resolveApiAssetUrl = (
  path: string
) => {
  if (!path) return "";

  if (/^https?:\/\//i.test(path)) {
    return path;
  }

  if (!apiOrigin) {
    return path;
  }

  return `${apiOrigin}${path.startsWith("/") ? path : `/${path}`}`;
};
