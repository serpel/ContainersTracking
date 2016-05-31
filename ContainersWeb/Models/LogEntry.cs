﻿using System;
using System.ComponentModel.DataAnnotations;

namespace ContainersWeb.Models
{
    public class LogEntry
    {
        [Key]
        public Int32 Id { get; set; }
        public DateTime Date { get; set; }
        public string Username { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public string Logger { get; set; }
        public string CallSite { get; set; }
        public string ServerName { get; set; }
        public string Port { get; set; }
        public string Url { get; set; }
        public string RemoteAddress { get; set; }
    }
}