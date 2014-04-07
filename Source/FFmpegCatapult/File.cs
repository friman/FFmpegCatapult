﻿// File properties interface for FFmpeg Catapult.
// Copyright (C) 2013 Myles Thaiss

// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FFmpegCatapult
{
    class File
    {
        // Variables
        private static string audio = "";
        private static string format = "";
        private static string input = "";
        private static string output = "";

        // Codecs and file extensions
        private static string[,] audioCodecs = new string[,]
        {
            {"AC3", "ac3"}, {"AAC", "aac"}, {"FLAC", "flac"}, {"HE-AAC", "heaac"},
            {"MP2", "mp2"}, {"MP3", "mp3"}, {"Opus", "opus"}, {"PCM", "pcm"}, 
            {"Speex", "speex"}, {"Vorbis", "vorbis"}, {"WMA", "wma"}, 
            {"Copy", "copy"}, {"None", "none"}
        };
        private static string[,] videoCodecs = new string[,]
        {
            {"H.264", "h264"}, {"MPEG-2", "mpeg2"}, {"MPEG-4", "mpeg4"},
            {"Theora", "theora"}, {"VP8", "vp8"}, {"WMV", "wmv"},
            {"Copy", "copy"}, {"None", "none"}
        };
        private static string[,] formats = new string[,]
        {
            {"AVI", "avi"}, {"M4A", "m4a"}, {"Matroska", "mkv"}, {"MP3", "mp3"}, 
            {"MP4", "mp4"}, {"MPEG Program Stream", "mpg"}, {"MPEG Transport Stream", "ts"},
            {"Ogg", "ogg"}, {"WebM", "webm"}, {"Windows Media Audio", "wma"},
            {"Windows Media Video", "wmv"}, {"Custom", "custom"}
        };

        // Property methods
        public static string AudioStream
        {
            get { return audio; }
            set { audio = value; }
        }

        public static string Format
        {
            get { return format; }
            set
            {
                format = value;

                switch (format)
                {
                    case "avi":
                        Audio.Codecs = audioCodecs;
                        Video.Codecs = videoCodecs;
                        if (!IsCodecSupported(Audio.Codec, Audio.Codecs))
                        {
                            Audio.Codec = "mp3";
                        }
                        if (!IsCodecSupported(Video.Codec, Video.Codecs))
                        {
                            Video.Codec = "mpeg4";
                        }
                        break;
                    case "m4a":
                        Audio.Codecs = new string[,] {
                            {"AAC", "aac"}, {"HE-AAC", "heaac"}, {"Copy", "copy"}
                        };
                        Video.Codecs = new string[,] {
                            {"None", "none"}
                        };
                        if (!IsCodecSupported(Audio.Codec, Audio.Codecs))
                        {
                            Audio.Codec = "aac";
                        }
                        break;
                    case "mkv":
                        Audio.Codecs = audioCodecs;
                        Video.Codecs = videoCodecs;
                        if (!IsCodecSupported(Audio.Codec, Audio.Codecs) | Audio.Codec == "wma")
                        {
                            Audio.Codec = "aac";
                        }
                        if (!IsCodecSupported(Video.Codec, Video.Codecs) | Video.Codec == "wmv")
                        {
                            Video.Codec = "h264";
                        }
                        break;
                    case "mp3":
                        Audio.Codecs = new string[,]
                        {
                            {"MP3", "mp3"}, {"Copy", "copy"}
                        };
                        Video.Codecs = new string[,]
                        {
                            {"None", "none"}
                        };
                        if (!IsCodecSupported(Audio.Codec, Audio.Codecs))
                        {
                            Audio.Codec = "mp3";
                        }
                        break;
                    case "mp4":
                        Audio.Codecs = new string[,]
                        {
                            {"AC-3", "ac3"}, {"AAC", "aac"}, {"HE-AAC", "heaac"},
                            {"MP3", "mp3"}, {"Copy", "copy"}, {"None", "none"}
                        };
                        Video.Codecs = new string[,]
                        {
                            {"H.264", "h264"}, {"MPEG-2", "mpeg2"}, {"MPEG-4", "mpeg4"},
                            {"Copy", "copy"}, {"None", "none"}
                        };
                        if (!IsCodecSupported(Audio.Codec, Audio.Codecs) | Audio.Codec == "mp3")
                        {
                            Audio.Codec = "aac";
                        }
                        if (!IsCodecSupported(Video.Codec, Video.Codecs))
                        {
                            Video.Codec = "mpeg4";
                        }
                        break;
                    case "mpg":
                        Audio.Codecs = new string[,]
                        {
                            {"MP2", "mp2"}, {"MP3", "mp3"}, {"Copy", "copy"}, {"None", "none"}
                        };
                        Video.Codecs = new string[,]
                        {
                            {"MPEG-2", "mpeg2"}, {"Copy", "copy"}
                        };
                        if (!IsCodecSupported(Audio.Codec, Audio.Codecs))
                        {
                            Audio.Codec = "mp2";
                        }
                        if (!IsCodecSupported(Video.Codec, Video.Codecs))
                        {
                            Video.Codec = "mpeg2";
                        }
                        break;
                    case "ts":
                        Audio.Codecs = new string[,]
                        {
                            {"AC-3", "ac3"}, {"AAC", "aac"}, {"MP2", "mp2"},
                            {"MP3", "mp3"}, {"Copy", "copy"}, {"None", "none"}
                        };
                        Video.Codecs = new string[,] {
                            {"H.264", "h264"}, {"MPEG-2", "mpeg2"}, {"Copy", "copy"}
                        };
                        if (!IsCodecSupported(Audio.Codec, Audio.Codecs))
                        {
                            Audio.Codec = "mp2";
                        }
                        if (!IsCodecSupported(Video.Codec, Video.Codecs))
                        {
                            Video.Codec = "mpeg2";
                        }
                        break;
                    case "ogg":                        
                        Audio.Codecs = new string[,] {
                            {"FLAC", "flac"}, {"Opus", "opus"}, {"Speex", "speex"},
                            {"Vorbis", "vorbis"}, {"Copy", "copy"}, {"None", "none"}
                        };                        
                        Video.Codecs = new string[,] {
                            {"Theora", "theora"}, {"Copy", "copy"}, {"None", "none"}
                        };
                        if (!IsCodecSupported(Audio.Codec, Audio.Codecs))
                        {
                            Audio.Codec = "vorbis";
                        }
                        if (!IsCodecSupported(Video.Codec, Video.Codecs))
                        {
                            Video.Codec = "theora";
                        }
                        break;
                    case "webm":                        
                        Audio.Codecs = new string[,] {
                            {"Vorbis", "vorbis"}, {"Copy", "copy"}, {"None", "none"}
                        };                        
                        Video.Codecs = new string[,] {
                            {"VP8", "vp8"}, {"Copy", "copy"}
                        };
                        if (!IsCodecSupported(Audio.Codec, Audio.Codecs))
                        {
                            Audio.Codec = "vorbis";
                        }
                        if (!IsCodecSupported(Video.Codec, Video.Codecs))
                        {
                            Video.Codec = "vp8";
                        }
                        break;
                    case "wma":
                        Audio.Codecs = new string[,] {
                            {"WMA", "wma"}, {"Copy", "copy"}
                        };
                        Video.Codecs = new string[,] {
                            {"None", "none"}
                        };
                        if (!IsCodecSupported(Audio.Codec, Audio.Codecs))
                        {
                            Audio.Codec = "wma";
                        }
                        break;
                    case "wmv":                        
                        Audio.Codecs = new string[,] {
                            {"WMA", "wma"}, {"Copy", "copy"}, {"None", "none"}
                        };                        
                        Video.Codecs = new string[,] {
                            {"WMV", "wmv"}, {"Copy", "copy"}
                        };
                        if (!IsCodecSupported(Audio.Codec, Audio.Codecs))
                        {
                            Audio.Codec = "wma";
                        }
                        if (!IsCodecSupported(Video.Codec, Video.Codecs))
                        {
                            Video.Codec = "wmv";
                        }
                        break;
                    default:
                        Audio.Codecs = audioCodecs;
                        Video.Codecs = videoCodecs;
                        break;
                }
            }
        }

        public static string[,] Formats
        {
            get { return formats; }
        }

        public static string Input
        {
            get { return input; }
            set { input = value; }
        }

        public static string Output
        {
            get { return output; }
            set
            {
                if (format == "custom")
                {
                    output = value;
                }
                else
                {
                    string outfile = "";

                    if (value != "")
                    {
                        outfile = Path.ChangeExtension(value, format);
                    }

                    // Avoid conflicting file names
                    if (outfile == input)
                    {
                        outfile = Path.ChangeExtension(value, null);
                        outfile = string.Format("{0}_output.{1}", outfile, format);
                    }

                    output = outfile;
                }
            }
        }

        // Supported codec check
        private static bool IsCodecSupported(string codec, string[,] codecs)
        {
            bool supported = false;

            for (int i = 0; i < codecs.GetLength(0); i++)
            {
                if (codec == codecs[i, 1])
                {
                    supported = true;
                    break;
                }
            }

            return supported;
        }
    }
}