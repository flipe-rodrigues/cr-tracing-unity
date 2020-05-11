%% initialization
clear;
clc;

%% change directory
levelsPath = uigetdir();
if levelsPath == 0
    return;
end
levelsDir = dir(levelsPath);
levelsDir = levelsDir([levelsDir.isdir]);
levelsDir = levelsDir(3:end);

%% stuff to fix
brk_expressions = {'",','},'};
tab_expressions = {'\n'};
server_id = 0;

% iterate through levels
n_levels = numel(levelsDir);
for ii = 1 : n_levels
    roomsPath = fullfile(levelsDir(ii).folder,levelsDir(ii).name);
    roomsDir = dir([roomsPath,filesep,'*.json']);
    
    %% iterate through rooms
    n_files = numel(roomsDir);
    for jj = 1 : n_files
        jsonFile = fullfile(roomsDir(jj).folder,roomsDir(jj).name);
        
        readfid = fopen(jsonFile,'r');
        raw = fread(readfid);
        fclose(readfid);
        
        str = char(raw');
        data = jsondecode(str);
        data.map = levelsDir(ii).name;
        data.server_id = ['r',num2str(server_id)];
%         data.room_id = data.id;
%         data = rmfield(data,'id');
%         data = rmfield(data,'db_id');
        if contains(data.map,'Second')
            data.position.z = +2;
        elseif contains(data.map,'Main')
            data.position.z = -1;
        else
            data.position.z = +1;
        end
        fields = fieldnames(data);
        offset = find(strcmpi(fields,'server_id'));
        data = orderfields(data,circshift([7,1,2,3,4,5,6],-offset));
        str = jsonencode(data);
        
        str = insertBefore(str,'"server_id"','\n');
        for kk = 1 : numel(brk_expressions)
            str = insertAfter(str,brk_expressions{kk},'\n');
        end
        for kk = 1 : numel(tab_expressions)
            str = insertAfter(str,tab_expressions{kk},'\t');
        end
        str = strrep(str,str(end-10:end),[str(end-10:end-1),'\n',str(end)]);
        
        savefid = fopen(jsonFile,'w');
        fprintf(savefid,str,'char');
        fclose(savefid);
        
        server_id = server_id + 1;
    end
end

%% back-end hack
savefile = 'backendhack.txt';
savefid = fopen(savefile,'w');

str = '$stmt->bind_param("s", $user,';
for ii = 0 : server_id - 1
    str = [str,'$r',num2str(ii),','];
    str = insertAfter(str,'"s','i');
end
str = [str(1:end-1),');'];
fprintf(savefid,str);
fprintf(savefid,'\n');

str = '$stmt=$conn->prepare("INSERT INTO traces (user,) VALUES (?)")';
for ii = server_id - 1 : -1 : 0
    r_substr = sprintf('r%i,',ii);
    str = insertAfter(str,'user,',r_substr);
    if (ii == server_id - 1) 
        str = strrep(str,r_substr,r_substr(1:end-1));
    end
    str = insertBefore(str,')"',',?');
end
fprintf(savefid,str);
fprintf(savefid,';\n');

for ii = 0 : server_id - 1
    fprintf(savefid,'$r%i=$_POST[''r%i''];\n',ii,ii);
end

fclose(savefid);