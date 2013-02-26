import os
import datetime
import subprocess

unity = '/Applications/Unity/Unity.app/Contents/MacOS/Unity'
cwd = os.getcwd()

print('Creating testing project directory to make Unity happy...')
cmd = [ "/usr/bin/rsync", "-r", "--exclude=.git", ".", "/tmp/testing.unity" ]
subprocess.call(cmd)
os.chdir("/tmp/testing.unity")
cwd = os.getcwd() # New, for testy

today = datetime.date.today()
output = cwd + "/Reports/" + str(today) + "__testResults.txt";

cmd = [ "/bin/rm", "-f", output ]
subprocess.call(cmd)

cmd = [ unity, "-batchmode", "-quit", "-projectPath", cwd, "-executeMethod", "Ps.TestSuite.RunTests", "testOutputPath="+output ]
subprocess.call(cmd)

try:
   with open(output) as f: pass
   cmd = [ "/bin/cat", output ]
   subprocess.call(cmd)
except IOError as e:
   print('Tests did not run. Try inpecting log file (~/Library/Logs/Unity/Editor.log)')
