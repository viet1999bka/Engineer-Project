using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DATN.Helpper
{
    public static class Utlities
    {
        public static string StripHTML(string input)
        {
            try
            {
                if (!string.IsNullOrEmpty(input))
                {
                    return Regex.Replace(input, "<.*?>", String.Empty);
                }
            }
            catch
            {
                return null;
            }
            return null;
        }
        public static bool IsValidEmail(string email)
        {
            if (email.Trim().EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static int PAGE_SIZE = 20;
        public static void CreateIfMissing(string path)
        {
            bool folderExists = Directory.Exists(path);
            if (!folderExists)
                Directory.CreateDirectory(path);
        }
        public static string ToTitleCase(string str)
        {
            string result = str;
            if (!string.IsNullOrEmpty(str))
            {
                var words = str.Split(' ');
                for (int index = 0; index < words.Length; index++)
                {
                    var s = words[index];
                    if (s.Length > 0)
                    {
                        words[index] = s[0].ToString().ToUpper() + s.Substring(1);
                    }
                }
                result = string.Join(" ", words);
            }
            return result;
        }
        public static bool IsInteger(string str)
        {
            Regex regex = new Regex(@"^[0-9]+$");

            try
            {
                if (String.IsNullOrWhiteSpace(str))
                {
                    return false;
                }
                if (!regex.IsMatch(str))
                {
                    return false;
                }

                return true;

            }
            catch
            {

            }
            return false;

        }
        public static string GetRandomKey(int length = 5)
        {
            //chuỗi mẫu (pattern)
            string pattern = @"0123456789zxcvbnmasdfghjklqwertyuiop[]{}:~!@#$%^&*()+";
            Random rd = new Random();
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                sb.Append(pattern[rd.Next(0, pattern.Length)]);
            }

            return sb.ToString();
        }
        public static string SEOUrl(string url)
        {
            url = url.ToLower();
            url = Regex.Replace(url, @"[áàạảãâấầậẩẫăắằặẳẵ]", "a");
            url = Regex.Replace(url, @"[éèẹẻẽêếềệểễ]", "e");
            url = Regex.Replace(url, @"[óòọỏõôốồộổỗơớờợởỡ]", "o");
            url = Regex.Replace(url, @"[íìịỉĩ]", "i");
            url = Regex.Replace(url, @"[ýỳỵỉỹ]", "y");
            url = Regex.Replace(url, @"[úùụủũưứừựửữ]", "u");
            url = Regex.Replace(url, @"[đ]", "d");

            //2. Chỉ cho phép nhận:[0-9a-z-\s]
            url = Regex.Replace(url.Trim(), @"[^0-9a-z-\s]", "").Trim();
            //xử lý nhiều hơn 1 khoảng trắng --> 1 kt
            url = Regex.Replace(url.Trim(), @"\s+", "-");
            //thay khoảng trắng bằng -
            url = Regex.Replace(url, @"\s", "-");
            while (true)
            {
                if (url.IndexOf("--") != -1)
                {
                    url = url.Replace("--", "-");
                }
                else
                {
                    break;
                }
            }
            return url;
        }
        public static async Task<string> UploadFile(Microsoft.AspNetCore.Http.IFormFile file, string sDirectory, string newname = null)
        {
            try
            {
                if (newname == null) newname = file.FileName;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", sDirectory);
                CreateIfMissing(path);
                string pathFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", sDirectory, newname);
                var supportedTypes = new[] { "jpg", "jpeg", "png", "gif" };
                var fileExt = System.IO.Path.GetExtension(file.FileName).Substring(1);
                if (!supportedTypes.Contains(fileExt.ToLower())) /// Khác các file định nghĩa
                {
                    return null;
                }
                else
                {
                    using (var stream = new FileStream(pathFile, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    return newname;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static async Task<string> UploadFileCode(Microsoft.AspNetCore.Http.IFormFile file, string sDirectory, string newname = null)
        {
            try
            {
                if (newname == null) newname = file.FileName;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "codeFile", sDirectory);
                CreateIfMissing(path);
                string pathFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "codeFile", sDirectory, newname);
                var fileExt = System.IO.Path.GetExtension(file.FileName).Substring(1);
                using (var stream = new FileStream(pathFile, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                return newname;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static void deleteFile(string Path)
        {
            if (File.Exists(Path))
            {
                try
                {
                    File.Delete(Path);
                }
                catch
                {
                    throw;
                }

            }
        }
        public static bool writeCodeFunction(string codeSubmit, string filesource, string filedest)
        {
            try
            {
                if (File.Exists(filedest)) File.Delete(filedest);
                File.Copy(filesource, filedest);
                using (StreamWriter writer = System.IO.File.AppendText(filedest))
                {
                    writer.WriteLine(codeSubmit);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool writeCodeClassPy(string codeSubmit, string path, string filename, string userID, string exten)
        {
            try
            {
                var pathFile = path + filename + userID + exten;
                if (File.Exists(pathFile)) File.Delete(pathFile);
                // Đọc code ra từ file chính ra
                string codeInFile = File.ReadAllText(path + filename + exten);

                // Thay thế tên class bằng tên file mới


                // Chèn code submit vào file 
                string oldText = "#Write solution here";
                string newText = oldText + "\n" + codeSubmit;

                if (codeInFile.Contains(oldText))
                {
                    codeInFile = codeInFile.Replace(oldText, newText);
                }

                File.WriteAllText(pathFile, codeInFile);

                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool writeCodeClassFunction(string codeSubmit, string path, string filename, string userID, string exten)
        {
            try
            {
                var pathFile = path + filename + userID + exten;
                if (File.Exists(pathFile)) File.Delete(pathFile);
                // Đọc code ra từ file chính ra
                string codeInFile = File.ReadAllText(path + filename + exten);

                // Thay thế tên class bằng tên file mới
                string oldText = "class " + filename;
                string newText = "class " + filename + userID;
                if (codeInFile.Contains(oldText))
                {
                    codeInFile = codeInFile.Replace(oldText, newText);
                }

                // Chèn code submit vào file 
                oldText = "/* Write solution here */";
                newText = oldText + "\n" + codeSubmit;

                if (codeInFile.Contains(oldText))
                {
                    codeInFile = codeInFile.Replace(oldText, newText);
                }

                File.WriteAllText(pathFile, codeInFile);

                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static List<string> getResults(string path, string fileMain, List<(string, string)> testCase, string codeSubmit, string userID, string languageExten, string timeLimit)
        {
            string res1 = "";
            List<string> result = new List<string>();
            try
            {
                var argrumStart = @"/C cd";
                // Tiến hành copy file và tạo tham số đầu vào dòng lệnh
                string nameFile = fileMain.Replace("." + languageExten, "") + userID;
                string filesource = path + "\\" + fileMain;
                string filedest = path + "\\" + fileMain.Replace("." + languageExten, "") + userID + "." + languageExten;
                string argrumTranslate;
                string argumExe;
                if (languageExten == "cpp")
                {
                    // Gọi hàm copy file để tiến hành copy file và ghi vào code submit
                    if (!writeCodeFunction(codeSubmit, filesource, filedest)) return null;
                    // Đặt tham số biên dịch để biên dịch file code sang exe
                    argrumTranslate = argrumStart + " " + path + " & g++ " + "-o " + nameFile + " " + nameFile + ".cpp";
                    argumExe = argrumStart + " " + path + " & .\\" + nameFile + ".exe";
                }
                else if (languageExten == "cs")
                {
                    if (!writeCodeClassFunction(codeSubmit, path + "\\", fileMain.Replace(".cs", ""), userID, ".cs")) return null;
                    argrumTranslate = argrumStart + " " + path + " & csc " + nameFile + ".cs";
                    argumExe = argrumStart + " " + path + " & .\\" + nameFile + ".exe";
                }
                else if (languageExten == "java")
                {
                    if (!writeCodeClassFunction(codeSubmit, path + "\\", fileMain.Replace(".java", ""), userID, ".java")) return null;
                    //argrumTranslate = argrumStart + " " + path + " & javac " + nameFile + ".java";
                    argrumTranslate = argrumStart + " " + path + " & javac " + nameFile + ".java";
                    argumExe = argrumStart + " " + path + " & java " + nameFile;
                }
                else if (languageExten == "py")
                {
                    if (!writeCodeClassPy(codeSubmit, path + "\\", fileMain.Replace(".py", ""), userID, ".py")) return null;
                    //argrumTranslate = argrumStart + " " + path + " & javac " + nameFile + ".java";
                    argrumTranslate = argrumStart + " " + path;
                    argumExe = argrumStart + " " + path + " & python " + nameFile + ".py";
                }
                else
                {
                    return null;
                }


                // Chạy tiến trình biên dịch file code sang exe
                Process process = new Process();
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.Arguments = argrumTranslate;
                process.StartInfo.CreateNoWindow = false;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.Start();
                process.WaitForExit();
                // Chạy từng test case với hàm exe để lấy kết quả
                // Tham số từng test case
                string argvExe = "";
                foreach (var item in testCase)
                {
                    // Khởi tạo tham số chứa từng test case 
                    argvExe = argumExe + " " + item.Item1;
                    // Tạo tiến trình và chạy file exe lấy kết quả
                    // Start the child process.
                    Process processExe = new Process();
                    processExe.StartInfo.FileName = "cmd.exe";
                    processExe.StartInfo.Arguments = argvExe;
                    processExe.StartInfo.CreateNoWindow = false;
                    processExe.StartInfo.RedirectStandardInput = true;
                    processExe.StartInfo.RedirectStandardOutput = true;
                    processExe.StartInfo.UseShellExecute = false;
                    string output = "";
                    int timeout = Int32.Parse(timeLimit);
                    Stopwatch stopwatch = new Stopwatch();
                    processExe.Start();
                    stopwatch.Start();

                    while (stopwatch.ElapsedMilliseconds < timeout)
                    {
                        if (processExe.HasExited)
                        {
                            try
                            {
                                output = processExe.StandardOutput.ReadToEnd();
                            }
                            catch
                            {
                                output = null;
                            }
                            break;
                        }
                    }
                    stopwatch.Stop();
                    stopwatch.Reset();

                    if (processExe.HasExited == false)
                    {
                        output = "Time out";
                        try
                        {
                            processExe.Kill(true);

                        }
                        catch
                        {
                            throw;
                        }
                    }
                    result.Add(output);
                    //processExe.WaitForExit();
                }

                // Xóa file code
                deleteFile(filedest);
                // xóa file class
                string fileClass = filedest.Replace(languageExten, "class");
                deleteFile(fileClass);
                // Xóa file Exe
                string fileExe = filedest.Replace(languageExten, "exe");
                deleteFile(fileExe);

            }
            catch
            {
                return null;
            }
            return result;
        }

        public static bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

        public static bool IsValidPass(string passwd)
        {
            // Ít nhất 8 ký tự và tối đa là 50
            if (passwd == null)
            {
                return false;
            }
            if (passwd.Length < 8 || passwd.Length > 50)
                return false;
            // Chứa ít nhất 1 chữ hoa
            if (!passwd.Any(char.IsUpper))
                return false;
            // Chứa ít nhất 1 chữ thường
            if (!passwd.Any(char.IsLower))
                return false;
            // Không chứa khoảng trắng
            if (passwd.Contains(" "))
                return false;

            // Không chứa ký tự đặc biệt
            string specialCh = @"%!@#$%^&*()?/>.<,:;'\|}]{[_~`+=-" + "\"";
            char[] specialChArray = specialCh.ToCharArray();
            foreach (char ch in specialChArray)
            {
                if (passwd.Contains(ch))
                    return true;
            }
            return false;
        }


    }
}
